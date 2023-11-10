using Assets.Scripts.Data.MapSets;
using Assets.Scripts.Models.Map;
using Assets.Scripts.Models.Map.Gizmos;
using Assets.Scripts.Models.Map.Spawners;
using Assets.Scripts.Models.Map.Triggers;
using Assets.Scripts.Simulation.SMath;
using Assets.Scripts.Unity.Map;
using Assets.Scripts.Utils;
using Il2CppSystem.Collections.Generic;

namespace NoOutlet {
    static class NoOutletMap {
        public static string Name = "No Outlet";

        private static MapDetails details = null;
        private static MapModel model = null;

        public static MapDetails Details {
            get {
                if (details == null) {
                    details = new MapDetails() {
                        id = Name,
                        isAvailable = true,
                        difficulty = MapDifficulty.Advanced,
                        coopMapDivisionType = CoopDivision.VERTICAL,
                        unlockDifficulty = MapDifficulty.Intermediate,
                        mapMusic = "MusicDarkA",
                        mapSprite = new SpriteReference() { guidRef = Name }
                    };
                }
                return details;
            }
        }

        public static MapModel Model {
            get {
                if (model == null) {
                    model = new MapModel(Name,
                        GetAreaModels(),
                        GetBlockerModels(),
                        GetCoopAreaLayoutModels(),
                        GetPathModels(),
                        GetRemoveableModels(),
                        GetMapGizmoModels(),
                        GetDifficultyModel(),
                        GetPathSpawnerModel(),
                        GetMapEventModels(),
                        GetBloonWideSpeed());
                }
                return model;
            }
        }

        #region Areas

        private static AreaModel[] GetAreaModels() => new AreaModel[] {
            GetAreaWhole(),
            GetAreaStraight(),
            GetAreaCircle(),
            GetAreaIsland()
        };

        private static AreaModel GetAreaWhole() {
            List<Vector2> initArea = new List<Vector2>();
            initArea.Add(new Vector2(-330, -330));
            initArea.Add(new Vector2(-330, 330));
            initArea.Add(new Vector2(330, 330));
            initArea.Add(new Vector2(330, -330));
            return new AreaModel("Whole", new Polygon(initArea), 0, AreaType.land);
        }

        private static AreaModel GetAreaStraight() {
            List<Vector2> initArea = new List<Vector2>();
            initArea.Add(new Vector2(-20, 30));
            initArea.Add(new Vector2(-20, 330));
            initArea.Add(new Vector2(20, 330));
            initArea.Add(new Vector2(20, 30));
            return new AreaModel("Straight", new Polygon(initArea), 0, AreaType.track);
        }

        private static AreaModel GetAreaCircle() {
            List<Vector2> initArea = new List<Vector2>();
            for (int i = 0; i < 360; i++) {
                Vector2 point = new Vector2(60f, 0);
                point.Rotate(i);
                point.y -= 25f;
                initArea.Add(point);
            }
            return new AreaModel("Circle", new Polygon(initArea), 0, AreaType.track);
        }

        private static AreaModel GetAreaIsland() {
            List<Vector2> initArea = new List<Vector2>();
            for (int i = 0; i < 360; i++) {
                Vector2 point = new Vector2(25, 0);
                point.Rotate(i);
                point.y -= 25;
                initArea.Add(point);
            }
            return new AreaModel("Island", new Polygon(initArea), 0, AreaType.land);
        }

        #endregion

        #region Blockers

        private static BlockerModel[] GetBlockerModels() => new BlockerModel[0];

        #endregion

        #region Coop Areas

        private static CoopAreaLayoutModel[] GetCoopAreaLayoutModels() => new CoopAreaLayoutModel[] {
            GetCoopAreaLayout2P(),
            GetCoopAreaLayout4P()
        };

        private static CoopAreaLayoutModel GetCoopAreaLayout2P() => new CoopAreaLayoutModel(new CoopAreaModel[] {
            GetCoopAreaLeft(),
            GetCoopAreaRight()
        }, AreaLayoutType.PLAYERS_2, new CoopAreaWhiteLineModel[0]);

        private static CoopAreaLayoutModel GetCoopAreaLayout4P() => new CoopAreaLayoutModel(new CoopAreaModel[] {
            GetCoopAreaTopLeft(),
            GetCoopAreaBottomLeft(),
            GetCoopAreaTopRight(),
            GetCoopAreaBottomRight()
        }, AreaLayoutType.PLAYERS_4, new CoopAreaWhiteLineModel[0]);

        private static CoopAreaModel GetCoopAreaLeft() {
            List<Vector2> initArea = new List<Vector2>();
            initArea.Add(new Vector2(-330, -330));
            initArea.Add(new Vector2(-330, 330));
            initArea.Add(new Vector2(0, 330));
            initArea.Add(new Vector2(0, -330));
            return new CoopAreaModel(0, new Polygon(initArea), new Vector2(-165, 0));
        }

        private static CoopAreaModel GetCoopAreaRight() {
            List<Vector2> initArea = new List<Vector2>();
            initArea.Add(new Vector2(0, -330));
            initArea.Add(new Vector2(0, 330));
            initArea.Add(new Vector2(330, 330));
            initArea.Add(new Vector2(330, -330));
            return new CoopAreaModel(1, new Polygon(initArea), new Vector2(165, 0));
        }

        private static CoopAreaModel GetCoopAreaTopLeft() {
            List<Vector2> initArea = new List<Vector2>();
            initArea.Add(new Vector2(-330, -330));
            initArea.Add(new Vector2(-330, 0));
            initArea.Add(new Vector2(0, 0));
            initArea.Add(new Vector2(0, -330));
            return new CoopAreaModel(0, new Polygon(initArea), new Vector2(-165, -165));
        }

        private static CoopAreaModel GetCoopAreaBottomLeft() {
            List<Vector2> initArea = new List<Vector2>();
            initArea.Add(new Vector2(-330, 0));
            initArea.Add(new Vector2(-330, 330));
            initArea.Add(new Vector2(0, 330));
            initArea.Add(new Vector2(0, 0));
            return new CoopAreaModel(1, new Polygon(initArea), new Vector2(-165, 165));
        }

        private static CoopAreaModel GetCoopAreaTopRight() {
            List<Vector2> initArea = new List<Vector2>();
            initArea.Add(new Vector2(0, -330));
            initArea.Add(new Vector2(0, 0));
            initArea.Add(new Vector2(330, 0));
            initArea.Add(new Vector2(330, -330));
            return new CoopAreaModel(2, new Polygon(initArea), new Vector2(165, -165));
        }

        private static CoopAreaModel GetCoopAreaBottomRight() {
            List<Vector2> initArea = new List<Vector2>();
            initArea.Add(new Vector2(0, 0));
            initArea.Add(new Vector2(0, 330));
            initArea.Add(new Vector2(330, 330));
            initArea.Add(new Vector2(330, 0));
            return new CoopAreaModel(3, new Polygon(initArea), new Vector2(165, 165));
        }

        #endregion

        #region Paths

        private static PathModel[] GetPathModels() => new PathModel[] { GetPath() };

        private static PathModel GetPath() {
            List<PointInfo> initPath = new List<PointInfo>();
            // Entrance
            initPath.Add(new PointInfo { point = new Vector3(-10, 330), bloonScale = 1, moabScale = 1 });
            initPath.Add(new PointInfo { point = new Vector3(-10, 30), bloonScale = 1, moabScale = 1 });
            // Main circle
            for (float i = 258.5f; i > -79.5; i--) {
                Vector3 point = new Vector3(50, 0);
                point.Rotate(i);
                point.y -= 25;
                initPath.Add(new PointInfo { point = point, bloonScale = 1, moabScale = 1 });
            }
            // Exit
            initPath.Add(new PointInfo { point = new Vector3(10, 30), bloonScale = 1, moabScale = 1 });
            initPath.Add(new PointInfo { point = new Vector3(10, 330), bloonScale = 1, moabScale = 1 });
            return new PathModel("Path", (PointInfo[])initPath.ToArray(), true, false,
                new Vector3(-1000, -1000, -1000), new Vector3(-1000, -1000, -1000), null, null);
        }

        #endregion

        #region Removeables

        private static RemoveableModel[] GetRemoveableModels() => new RemoveableModel[0];

        #endregion

        #region Gizmos

        private static MapGizmoModel[] GetMapGizmoModels() => new MapGizmoModel[0];

        #endregion

        private static DifficultyModel GetDifficultyModel() => new DifficultyModel("", 2);

        #region Path Spawners

        private static PathSpawnerModel GetPathSpawnerModel() => new PathSpawnerModel(""
            , GetForwardSplitterModel(), GetReverseSplitterModel());

        private static SplitterModel GetForwardSplitterModel() => new SplitterModel("", new string[] { "Path" });

        private static SplitterModel GetReverseSplitterModel() => new SplitterModel("", new string[] { "Path" });

        #endregion

        #region Events

        private static MapEventModel[] GetMapEventModels() => new MapEventModel[0];

        #endregion

        private static float GetBloonWideSpeed() => 1;
    }
}
