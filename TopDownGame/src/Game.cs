using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.IO;
using System.Threading;

namespace TopDownGame
{
    static class Game
    {
        // list of game scenes
        private static Stack<Scene> scenes;

        // stores whether to display collision information
        public static bool Debug = false;

        // filenames of the levels
        public static List<string> Levels = new List<string> { "maps/map1.txt", "maps/map2.txt", "maps/map3.txt", "maps/map4.txt" };
        // the index of the current level
        private static int LevelIndex = 0;

        // whether to change level at the end of the update cycle
        private static bool nextLevel = false;
        // whether to reset level at the end of the update cycle
        private static bool resetLevel = false;

        public static void NextLevel()
        {
            nextLevel = true;
        }

        public static void ResetLevel()
        {
            resetLevel = true;
        }

        // gets filename of the current level
        public static string CurLevel
        { get
            {
                return Levels[LevelIndex];
            }
        }

        // gets the current scene off the stack of scenes
        public static Scene Scene
        { get
            {
                return scenes.Peek();
            }
        }

        /// <summary>
        /// Parses the level file to create a scene and creates or resizes the window
        /// </summary>
        /// <param name="fileMap">the file that needs to be parsed</param>
        public static void LoadLevel(string fileMap)
        {
            Scene scene = new Scene();
            scenes.Push(scene);

            // parse scene
            StreamReader streamReader = new StreamReader(fileMap);
            string line;
            string state = "";
            uint tileLine = 0;
            string image = "";

            int tileSize = int.Parse(streamReader.ReadLine());
            int width = int.Parse(streamReader.ReadLine());
            int height = int.Parse(streamReader.ReadLine());

            TileMap tileMap = new TileMap(width, height, tileSize);
            Path curPath = new Path(0);

            while ((line = streamReader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line == "#TILES")
                {
                    scene.GameObjects.Add(tileMap);
                    state = "tiles";
                    tileLine = 0;
                }
                else if (line == "#PLAYER")
                {
                    state = "player";
                    tileLine = 0;
                    image = streamReader.ReadLine().Trim();
                }
                else if (line == "#FSMGUARD")
                {
                    state = "fsmGuard";
                    tileLine = 0;
                    image = streamReader.ReadLine().Trim();
                    curPath = new Path(int.Parse(streamReader.ReadLine()));
                }
                else if (line == "#TREEGUARD")
                {
                    state = "treeGuard";
                    tileLine = 0;
                    image = streamReader.ReadLine().Trim();
                    curPath = new Path(int.Parse(streamReader.ReadLine()));
                }
                else if (line == "#GOAL")
                {
                    state = "goal";
                    tileLine = 0;
                }
                else if (state == "tiles")
                {
                    for (int i = 0; i < line.Length; i++)
                        tileMap.tiles[tileLine, i] = line[i];
                    tileLine++;
                }
                else if (state == "player")
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        switch (line[i])
                        {
                            case 'p':
                                {
                                    scene.GameObjects.Add(new Player(new Vector2(i * tileSize + tileSize / 2, tileLine * tileSize + tileSize / 2), new Vector2(tileSize, tileSize), image));
                                    break;
                                }
                        }
                    }
                    tileLine++;
                }
                else if (state == "fsmGuard")
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == 'A')
                        {
                            scene.GameObjects.Add(new FSMGuard(new Vector2(i * tileSize + tileSize / 2, tileLine * tileSize + tileSize / 2), new Vector2(tileSize, tileSize), curPath, image));
                            break;
                        }
                        else if (char.IsDigit(line[i]))
                        {
                            curPath.Points[int.Parse(line[i].ToString()) - 1] = new Vector2(i * tileSize + tileSize / 2, tileLine * tileSize + tileSize / 2);
                        }
                    }
                    tileLine++;
                }
                else if (state == "treeGuard")
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == 'A')
                        {
                            scene.GameObjects.Add(new TreeGuard(new Vector2(i * tileSize + tileSize / 2, tileLine * tileSize + tileSize / 2), new Vector2(tileSize, tileSize), curPath, image));
                            break;
                        }
                        else if (char.IsDigit(line[i]))
                        {
                            curPath.Points[int.Parse(line[i].ToString()) - 1] = new Vector2(i * tileSize + tileSize / 2, tileLine * tileSize + tileSize / 2);
                        }
                    }
                    tileLine++;
                }
                else if (state == "goal")
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == 'G')
                        {
                            scene.GameObjects.Add(new GoalObject(new Vector2(i * tileSize + tileSize / 2, tileLine * tileSize + tileSize / 2), new Vector2(tileSize, tileSize)));
                            break;
                        }
                    }
                    tileLine++;
                }
            }
            CollisionTileMap collision = (CollisionTileMap)tileMap.CollisionShape;
            collision.UpdateTileMap();

            streamReader.Close();

            if (Window.IsCreated)
                Window.SetSize(new SFML.System.Vector2f(width * tileSize, height * tileSize));
            else
                Window.CreateWindow((uint)(width * tileSize), (uint)(height * tileSize), "Stealth Game");
        }

        /// <summary>
        /// Initialises the game
        /// </summary>
        public static void Init()
        {
            scenes = new Stack<Scene>();
            LoadLevel(CurLevel);

            Window.window.SetFramerateLimit(60);
        }

        /// <summary>
        /// Changes the curent scene to the next level
        /// </summary>
        private static void ChangeLevelToNextLevel()
        {
            Thread.Sleep(300);
            LevelIndex++;
            if (LevelIndex >= Levels.Count)
                LevelIndex = 0;

            scenes.Pop();
            LoadLevel(CurLevel);

            nextLevel = false;
            resetLevel = false;
        }

        /// <summary>
        /// Will reset the current level 
        /// </summary>
        private static void ChangeLevelToResetLevel()
        {
            Thread.Sleep(300);
            scenes.Pop();
            LoadLevel(CurLevel);

            nextLevel = false;
            resetLevel = false;
        }

        /// <summary>
        /// Updates the current scene and will change the scenes
        /// </summary>
        public static void Update()
        {
            scenes.Peek().Update();

            // test if the scene needs to be changed
            if (nextLevel)
                ChangeLevelToNextLevel();
            else if (resetLevel)
                ChangeLevelToResetLevel();
        }

        /// <summary>
        /// Runs the render function for the current scene
        /// </summary>
        public static void Render()
        {
            scenes.Peek().Render();
        }

        /// <summary>
        /// Cleans up the scenes
        /// </summary>
        public static void OnClose()
        {
            scenes.Clear();
        }
    }
}
