using Kodama.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Kodama.Editor {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TileObject), true)]
    public class SnapToGridEditor : UnityEditor.Editor {
        private TileObject _tileObject;
        private Tilemap _tilemap;

        private void OnSceneGUI() {
            _tileObject = target as TileObject;

            if (_tileObject == null) {
                return;
            }

            _tilemap = _tileObject.tilemap;
            
            if (_tilemap == null) {
                var grid = GameObject.Find("Grid");
                if (grid) {
                    _tilemap = grid.transform.GetChild(0).GetComponent<Tilemap>();
                    _tileObject.tilemap = _tilemap;
                }
                return;
            }

            var pos = _tilemap.WorldToCell(_tileObject.transform.position);
            _tileObject.transform.position = _tilemap.GetCellCenterWorld(pos);
        }
    }
}