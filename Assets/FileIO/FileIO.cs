using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;


class FileIO : MonoBehaviour
{
   static string chuckFileSTD = "chunkData.ck";
    public static string gameLevelFolder = Application.dataPath + "/levels/";
    

    public void createFolder(string folderName)
    {
       if(!Directory.Exists(gameLevelFolder + folderName))
        {
            Directory.CreateDirectory(gameLevelFolder + folderName);
        }
    }
    

    //void writeChunkData(string actualLevel,byte[]data)
    void writeChunkData(string actualLevel)
    {
        int chunkRow = 50;
        int chunkCol = 210;
        createFolder(actualLevel);
          // StreamWriter sw = new StreamWriter(gameLevelFolder + actualLevel + "/" + chuckFileSTD, true);
          FileStream stream = new FileStream(gameLevelFolder + actualLevel + "/" + chuckFileSTD, FileMode.Create, FileAccess.Write);
        BinaryWriter writer = new BinaryWriter(stream);


        
        System.Random rnd = new System.Random();

        byte randomNumber = 0;
        //byte[520] numbers;
        
        bool air = false;
        for (int i = 0; i < chunkRow; i++)
        {
            if(i > 42)
            {
                air = true;
            }
            for (int e = 0; e < chunkCol; e++)
            {
                //data += "\n!";
                for (int o = 0; o < 520; o++)
                {
                    if (air == true)
                    {
                        randomNumber = Convert.ToByte(0);
                    }else
                    {
                        randomNumber = Convert.ToByte(rnd.Next(0, 6));
                        //randomNumber = Convert.ToByte(2);
                    }
                    
                    writer.Write(randomNumber);
                }
            }
        }
        /*
        //TerrainGeneration TG = new TerrainGeneration();
        byte [,] blocksA = TerrainGeneration.TrueGenTerrain();
        foreach (var item in blocksA)
        {
            writer.Write(item);
        }
        */
       
        writer.Close();
        stream.Close();
        
        Debug.Log("Generado Terreno a fichero");
    }

    public static void readFile(string actualLevel, out byte[] letters)
    {
        
        //byte[] letters;
        FileStream stream = new FileStream(gameLevelFolder + actualLevel + "/" + chuckFileSTD, FileMode.Open, FileAccess.Read);
        BinaryReader reader = new BinaryReader(stream);

        letters = new byte[reader.BaseStream.Length];
        for(long i =0; i< reader.BaseStream.Length; i++)
        {
            letters[i] = reader.ReadByte();
        }
       
        reader.Close();
        stream.Close();
       // return letters;
    }
    //The last "/" between the file and the path shouldn't be used
    public static void doBackupFile(bool delete, string actualLevel = "0")
    {
        if(actualLevel == "0")
        {
            actualLevel = gameLevelFolder+ actualLevel;
        }else
        {
            actualLevel = gameLevelFolder+ actualLevel + "/";
        }

        string filenameWithoutExtension = Path.GetFileNameWithoutExtension(chuckFileSTD);

        string OriginalFile = actualLevel + filenameWithoutExtension + ".ck";
        string FileToReplace = actualLevel + filenameWithoutExtension + ".bck";
        string BackUpOfFileToReplace = actualLevel + filenameWithoutExtension + ".bck2";

        if (!File.Exists(actualLevel + Path.GetFileNameWithoutExtension(chuckFileSTD) + ".bck"))
        {
            File.Copy(@"" + actualLevel + chuckFileSTD, @"" + actualLevel + filenameWithoutExtension + ".bck");
            if (delete)
            {
                File.Delete(actualLevel + chuckFileSTD);
            }
        }else
        {
            if (delete) { 
            // Replace the file.
            File.Replace(OriginalFile, FileToReplace, BackUpOfFileToReplace);
            }else
            {
                File.Delete(BackUpOfFileToReplace);
                File.Move(FileToReplace, BackUpOfFileToReplace);
                File.Copy(OriginalFile, FileToReplace);
            }
            //Do the new bck
        }
        
    }
    void Start()
    {
        //createFolder("level1");
        //createFolder("level2");
        writeChunkData("test");
        //readFile();
        //doBackupFile(false, "test");
        //StartCoroutine(writeChunkData("test"));

        /* byte[] terrain; 
         readFile("test",out terrain);

         if(terrain.Length == 5460000)
         {
             Debug.LogWarning("True 5,460,000 blocks allocated, " + terrain.Length.ToString());
         }else Debug.LogWarning(terrain.Length);
         */
        //Debug.LogWarning("dONE");
        //CubeChunkComposer.composeCube();
        //CubeChunkComposer.composeChunks();
    }
}

