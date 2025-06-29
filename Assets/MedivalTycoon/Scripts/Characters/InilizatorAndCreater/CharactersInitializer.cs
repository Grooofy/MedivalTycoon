using UnityEngine;

namespace Characters
{
    public class CharactersInitializer 
    {
        public void InitCharacters(ICharacter character, Worker worker)
        {
            character.Initialize(worker);
        }
    }
}