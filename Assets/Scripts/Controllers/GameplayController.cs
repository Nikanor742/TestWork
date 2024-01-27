using System;

using UnityEngine;

using CookingPrototype.Kitchen;
using CookingPrototype.UI;

using JetBrains.Annotations;
using System.Collections;

namespace CookingPrototype.Controllers {
	public sealed class GameplayController : MonoBehaviour {
		public static GameplayController Instance { get; private set; }

		public GameObject TapBlock   = null;
		public WinWindow  WinWindow  = null;
		public LoseWindow LoseWindow = null;

		public StartWindow StartWindow = null;


		int _ordersTarget = 0;

		public int OrdersTarget {
			get { return _ordersTarget; }
			set {
				_ordersTarget = value;
				TotalOrdersServedChanged?.Invoke();
			}
		}

		public int        TotalOrdersServed { get; private set; } = 0;

		public event Action TotalOrdersServedChanged;

		void Awake() {
			if ( Instance != null ) {
				Debug.LogError("Another instance of GameplayController already exists");
			}
			Instance = this;
			StartCoroutine(InitTotalOrdersCount());
		}

		IEnumerator InitTotalOrdersCount() {
			yield return new WaitForSeconds(0.01f);
			if ( OrdersTarget != 0) {
				StartWindow?.Show();
			}
			else StartCoroutine(InitTotalOrdersCount());
		}

		void OnDestroy() {
			if ( Instance == this ) {
				Instance = null;
			}
		}

		void Init() {
			TotalOrdersServed = 0;
			Time.timeScale = 1f;
			TotalOrdersServedChanged?.Invoke();
		}

		public void CheckGameFinish() {
			if ( CustomersController.Instance.IsComplete ) {
				EndGame(TotalOrdersServed >= OrdersTarget);
			}
		}

		void EndGame(bool win) {
			Time.timeScale = 0f;
			TapBlock?.SetActive(true);
			if ( win ) {
				WinWindow.Show();
			} else {
				LoseWindow.Show();
			}
		}

		void HideWindows() {
			TapBlock?.SetActive(false);
			WinWindow?.Hide();
			LoseWindow?.Hide();
			StartWindow?.Hide();
		}

		[UsedImplicitly]
		public bool TryServeOrder(Order order) {
			if ( !CustomersController.Instance.ServeOrder(order) ) {
				return false;
			}

			TotalOrdersServed++;
			TotalOrdersServedChanged?.Invoke();
			CheckGameFinish();
			return true;
		}

		public void StartGame() {
			HideWindows();
			Time.timeScale = 1f;
		}

		public void Restart() {
			Init();
			CustomersController.Instance.Init();
			HideWindows();

			foreach ( var place in FindObjectsOfType<AbstractFoodPlace>() ) {
				place.FreePlace();
			}
			StartWindow?.Show();
		}

		public void CloseGame() {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
