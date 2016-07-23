using System;

namespace EnvMapGen
{
	class MainClass
	{
		public static Random rnd = new Random();
		public static int mapSize = 8;
		public static km[,] map = new km[mapSize, mapSize];
		public static int[] cursor = new int[2] {0, 0};
		public static void Main (string[] args)
		{
			bool drawEnv = true;

			CreateEnv ();
			CreateAltitude ();
			CheckForUndefined ();

			while (drawEnv) {
				Console.Clear();
				Console.WriteLine (mapSize+" km2 map\n");
				Console.WriteLine(DrawEnv ());
				MoveCursor ();
			}
		}

		static void CreateEnv(){
			// weather and env generation

			for (int i = 0; i < mapSize; i++) {
				for (int ii = 0; ii < mapSize; ii++) {
					map [i, ii] = new km (); // initializing array object
					bool isUndef = true;
					while (isUndef) {
						int temp = rnd.Next (-10, 50);
						int humidity = rnd.Next (0, 100);
						bool isWater = false;
						isUndef = false;

						if (rnd.Next (0, 4) == 2) {
							isWater = true;
						}

						map [i, ii].temp = temp;
						map [i, ii].humidity = humidity;
						if (isWater) {
							map [i, ii].biome = "Water";
							map [i, ii].humidity = rnd.Next (90,100);
							map [i, ii].Height = rnd.Next(-10,10);
							if (temp < 0) {
								map [i, ii].biome = "Ice";
								map [i, ii].humidity = rnd.Next (80,100);
							}
						} else if (temp > 10 && temp < 48 && humidity > 10 && humidity < 70) {
							map [i, ii].biome = "Grassland";
						} else if (temp > -10 && temp < 2 && humidity > 85) {
							map [i, ii].biome = "Snow";
						} else if (rnd.Next(0,5) == 2) {
							map [i, ii].biome = "Wasteland";
						} else if (temp > 20 && temp < 40 && humidity > 70 && humidity < 95) {
							map [i, ii].biome = "Jungle";
						} else if (temp > 40 && humidity > 8 && humidity < 50) {
							map [i, ii].biome = "Desert";
						} else {
							map [i, ii].biome = "Undefined";
							isUndef = true;
						}

						if(rnd.Next(0,4) == 2 ){
							map[i, ii].population = rnd.Next(0,100);
							if(map[i, ii].population >= 10 && map[i, ii].population <= 50){
								map[i, ii].popType = "Village";
							} else if(map[i, ii].population > 50){
								map[i, ii].popType = "Large Village";
							} else if(map[i, ii].population >= 1 && map[i, ii].population < 10){
								map[i ,ii].popType = "Populated";
							} else { map[i ,ii].popType = ""; }

							if(rnd.Next(0,4) == 2){
								map[i, ii].population = rnd.Next(0, 500);
								if(map[i, ii].population > 150){ map[i, ii].popType = "Town"; }
							}
						}

						if(temp >= -10 && temp <= 5){
							map[i, ii].prefix2 = "Freezing ";
						} else if(temp > 5 && temp <= 20){
                                                        map[i, ii].prefix2 = "Cold ";
                                                } else if(temp > 30 && temp <= 40){
                                                        map[i, ii].prefix2 = "Warm ";
                                                } else if(temp > 40){
                                                        map[i, ii].prefix2 = "Burning ";
                                                }

						if(humidity < 30){
                                                        map[i, ii].prefix3 = "Dry ";
                                                } else if(humidity > 75){
                                                        map[i, ii].prefix3 = "Humid ";
                                                }
						if(map [i, ii].biome == "Water" || map [i, ii].biome == "Ice"){
							map[i, ii].prefix = "";
							map[i, ii].prefix2 = "";
							map[i, ii].prefix3 = "";
							if(map [i, ii].biome == "Water"){
								map[i, ii].popType = "";
								map[i, ii].population = 0;
							}
						}
					}
				}
			}
		}

		static void CheckForUndefined(){
			for (int i = 0; i < mapSize; i++) {
				for (int ii = 0; ii < mapSize; ii++) {
					if (map [i, ii].biome == "Undefined") {
						Console.WriteLine ("Undefined at " + i + ", " + ii + "  -> Temp: " + map [i, ii].temp + " C / Humidity: " + map [i, ii].humidity + " %");
					}
				}
			}
		}

		static void CreateAltitude(){
			// altitude generation
			for (int i = 0; i < mapSize; i++) {
				for (int ii = 0; ii < mapSize; ii++) {
					map [i, ii].Height = rnd.Next (-300,3000);
				}
			}

			for (int i = 0; i < mapSize; i++) {
				for (int ii = 0; ii < mapSize; ii=ii+2) {
					if (ii < mapSize && ii > 1) {
						map [i, ii].Height = (map [i, ii + 1].Height + map [i, ii - 1].Height) / 2;
						if(rnd.Next(0,2) == 1 && map[i, ii].Height > 1500){map[i, ii].prefix = "Mountainous ";}
						if(rnd.Next(0,2) == 1 && map[i, ii].Height > 200){map[i, ii].prefix = "Flat ";}
					}
				}
			}
		}

		static void MoveCursor(){
			ConsoleKeyInfo input = Console.ReadKey();

			if (input.KeyChar == 'w') {
				if (cursor [0] - 1 >= 0) {
					cursor [0] -= 1;
				}
			} else if (input.KeyChar == 's') {
				if (cursor [0] + 1 < mapSize) {
					cursor [0] += 1;
				}
			} else if (input.KeyChar == 'a') {
				if (cursor [1] - 1 >= 0) {
					cursor [1] -= 1;
				}
			} else if (input.KeyChar == 'd') {
				if (cursor [1] + 1 < mapSize) {
					cursor [1] += 1;
				}
			}
		}


		static string DrawEnv(){
			string drawBuffer = " ";

			// draw code
			string b;
			char fc;
			for (int i = 0; i < mapSize; i++) {
				for (int ii = 0; ii < mapSize; ii++) {
					b = map [i, ii].biome;
					fc = b [0];
					if (cursor [0] == i && cursor [1] == ii) {
						if(drawBuffer.Length > 0){
							drawBuffer = drawBuffer.Remove(drawBuffer.Length - 1);
						}
						drawBuffer += "["+fc+"]";
					} else {
						drawBuffer += fc+" ";
					}
				}
				drawBuffer += "\n ";
			}

			km cmap = map[cursor[0], cursor[1]];
			drawBuffer += "\n\n";
			drawBuffer += cmap.prefix+cmap.prefix2+cmap.prefix3+cmap.biome+"\n";
			drawBuffer += "Temperature: "+map[cursor[0], cursor[1]].temp+" C\n";
			drawBuffer += "Humidity: "+map[cursor[0], cursor[1]].humidity+"%\n";
			if(cmap.popType != null){
				if(cmap.popType.Length > 1){
					drawBuffer += "Population: "+cmap.population+"  `"+cmap.popType+"`"+"\n";
				} else {
					drawBuffer += "Population: "+cmap.population+"\n";
				}
			} else {
				drawBuffer += "Population: "+cmap.population+"\n";
			}

			drawBuffer += "Altitude: "+map[cursor[0], cursor[1]].Height+" Meters\n";

			return drawBuffer;

		}
	}

	class km{
		public int temp, humidity, population, Height = 0;
		public string biome, popType, prefix, prefix2 , prefix3= "";
	}
}
