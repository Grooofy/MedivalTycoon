using Characters;
using UnityEngine;

namespace MedivalTycoon
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private CharacterManager _characterManager;
        [SerializeField] private LoadingGameSettings _loadingGameSettings;
        [SerializeField] private Wallet _wallet;
        
        
        private void Start()
        {
            _characterManager.CreateCharacters();
            _wallet.Initilize(_loadingGameSettings);
        }

        private void Update()
        {
            _characterManager.MoveCharacter();
        }
    }
}