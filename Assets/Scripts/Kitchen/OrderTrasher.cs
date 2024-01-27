using System;

using UnityEngine;

using JetBrains.Annotations;

namespace CookingPrototype.Kitchen {
	[RequireComponent(typeof(OrderPlace))]
	public sealed class OrderTrasher : MonoBehaviour {

		OrderPlace _place = null;
		float     _timer = 0f;

		const float _doubleTapTime = 0.4f;

		void Start() {
			_place = GetComponent<OrderPlace>();
		}

		/// <summary>
		/// Освобождает место по двойному тапу если еда на этом месте сгоревшая.
		/// </summary>
		[UsedImplicitly]
		public void TryTrashOrder(){
			if ( _timer == 0 ) {
				_timer = Time.realtimeSinceStartup;
			}
			else {
				if ( Time.realtimeSinceStartup - _timer < _doubleTapTime ) {
					_place.FreePlace();
				}
				_timer = 0;
			}
		}
	}
}
