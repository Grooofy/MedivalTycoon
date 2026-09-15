"""Run with Python 3 from the repository root; no third-party dependencies."""
from pathlib import Path
import json
import re

ROOT = Path(__file__).resolve().parents[2]
ASSETS = ROOT / "Assets/MedivalTycoon"


def scalar(value):
    value = value.strip()
    if value.startswith('"'):
        value = re.sub(r"\r?\n\s*", " ", value)
        value = re.sub(r"\\x([0-9a-fA-F]{2})", r"\\u00\1", value)
        return json.loads(value)
    return value


def validate_entries(dictionaries):
    errors = []
    tables = {}
    for language, entries in dictionaries.items():
        table = {}
        for key, value in entries:
            if not key or not value.strip():
                errors.append(f"{language}: empty key/text {key}")
            if key in table:
                errors.append(f"{language}: duplicate {key}")
            table[key] = value
        tables[language] = table
    keys = set().union(*(set(table) for table in tables.values()))
    for key in keys:
        parameters = []
        for language, table in tables.items():
            if key not in table:
                errors.append(f"{language}: missing {key}")
                continue
            text = table[key].replace("{{", "").replace("}}", "")
            parameters.append(set(re.findall(r"\{(\d+)(?:[,}:])", text)))
        if parameters and any(p != parameters[0] for p in parameters):
            errors.append(f"argument mismatch: {key}")
    return errors, keys


def main():
    dictionaries = {}
    for language in ("ru", "en", "tr"):
        text = (ASSETS / f"Resources/Localization/{language}.asset").read_text(encoding="utf-8-sig")
        dictionaries[language] = [(k, scalar(v)) for k, v in re.findall(
            r"(?m)^  - ID: ([^\r\n]+)\r?\n    Text: ([^\r\n]+)", text)]
        assert len(dictionaries[language]) >= 64, f"Empty/incomplete dictionary: {language}"
    errors, keys = validate_entries(dictionaries)
    guid = re.search(r"guid: (\w+)", (ASSETS / "Scripts/Localization/LocalizedText.cs.meta").read_text())[1]
    bindings = 0
    tutorial_keys = []
    for path in list((ASSETS / "Scenes").glob("*.unity")) + list((ASSETS / "Prefabs").rglob("*.prefab")):
        text = path.read_text(encoding="utf-8-sig")
        docs = re.split(r"(?=^--- !u!)", text, flags=re.M)[1:]
        by_id = {re.match(r"--- !u!\d+ &(-?\d+)", doc)[1]: doc for doc in docs}
        assert len(by_id) == len(docs), f"Duplicate Unity object ID: {path}"
        localized_objects = set()
        for doc in docs:
            if f"guid: {guid}" not in doc:
                continue
            component = re.match(r"--- !u!\d+ &(\d+)", doc)[1]
            owner = re.search(r"m_GameObject: \{fileID: (\d+)\}", doc)[1]
            key = re.search(r"_key: (\S+)", doc)[1]
            assert key in keys, f"Unknown key {key} in {path}"
            assert f"component: {{fileID: {component}}}" in by_id[owner], f"Unattached binding in {path}"
            first = re.search(r"component: \{fileID: (\d+)\}", by_id[owner])[1]
            assert re.match(r"--- !u!(?:4|224) &", by_id[first]), f"Transform must remain first: {path}"
            localized_objects.add(owner)
            bindings += 1
        for doc in docs:
            for match in re.finditer(r'(?m)^  m_[tT]ext: ("(?:[^"\\]|\\.)*"|[^\r\n]*)', doc):
                value = scalar(match[1])
                if not value or not any(c.isalpha() for c in value) or value == "Medival Tycoone":
                    continue
                owner = re.search(r"m_GameObject: \{fileID: (\d+)\}", doc)[1]
                assert owner in localized_objects, f"Unlocalized player text in {path}: {value!r}"
        for key in re.findall(r"MessageKey: (\S+)", text):
            assert key in keys, f"Unknown tutorial key: {key}"
            tutorial_keys.append(key)
        assert not re.search(r"(?m)^    Message:", text), f"Unmigrated tutorial messages: {path}"
    assert len(tutorial_keys) == len(set(tutorial_keys)) == 22
    assert set(tutorial_keys) == {k for k in keys if k.startswith("tutorial.")}
    for path in (ASSETS / "Scripts").rglob("*.cs"):
        text = path.read_text(encoding="utf-8-sig", errors="replace")
        for key in re.findall(r'(?:Get|Format|Bind)\([^\r\n]*?"([a-z][A-Za-z0-9_.]+)"', text):
            if key not in keys:
                errors.append(f"{path}: unknown key {key}")
    assert not errors, "\n".join(errors)
    for bad in [
        {"ru": [("k", "A")], "en": []},
        {"ru": [("k", "A"), ("k", "B")], "en": [("k", "A")]},
        {"ru": [("k", "{0}")], "en": [("k", "{1}")]},
    ]:
        assert validate_entries(bad)[0], "Validator accepted a broken dictionary"
    print(f"PASS: {len(keys)} keys x 3 languages; {bindings} UI bindings; 22 tutorial steps; code references and regression checks.")


if __name__ == "__main__":
    main()
