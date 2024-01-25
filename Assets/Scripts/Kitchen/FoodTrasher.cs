using System;

using UnityEngine;

using JetBrains.Annotations;

namespace CookingPrototype.Kitchen {
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodTrasher : MonoBehaviour {

		FoodPlace _place = null;
		float     _timer = 0f;

		const float _doubleTapTime = 0.3f;

		void Start() {
			_place = GetComponent<FoodPlace>();
		}

		/// <summary>
		/// Освобождает место по двойному тапу если еда на этом месте сгоревшая.
		/// </summary>
		[UsedImplicitly]
		public void TryTrashFood(){
			if ( _place.CurFood.CurStatus == Food.FoodStatus.Overcooked ){
				if(_timer == 0 ) {
					_timer = Time.realtimeSinceStartup;
				}
				else {
					if( Time.realtimeSinceStartup - _timer < _doubleTapTime ) {
						_place.ExtractFood();
					}
					_timer = 0;
				}
			}
		}
	}
}
