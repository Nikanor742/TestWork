using UnityEngine;
using UnityEngine.UI;

using CookingPrototype.Controllers;

using TMPro;

namespace CookingPrototype.UI {
	public sealed class StartWindow : MonoBehaviour {
		public TMP_Text GoalText = null;
		public Button OkButton = null;

		bool _isInit = false;

		void Init() {
			var gc = GameplayController.Instance;

			OkButton.onClick.AddListener(gc.StartGame);
		}

		public void Show() {
			if ( !_isInit ) {
				Init();
			}

			Time.timeScale = 0f;
			var gc = GameplayController.Instance;

			GoalText.text = gc.OrdersTarget.ToString();

			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}

		private void OnDestroy() {
			OkButton.onClick.RemoveAllListeners();
		}
	}
}
