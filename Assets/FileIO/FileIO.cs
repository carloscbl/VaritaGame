using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;


class FileIO : MonoBehaviour
{
    string chuckFileSTD;
    public string gameLevelFolder;
    
    void Start()
    {
        chuckFileSTD = "chunkData.ck";
        gameLevelFolder = Application.dataPath + "/levels/";
        writeChunkData("test");
    }
    public void createFolder(string folderName)
    {
       if(!Directory.Exists(gameLevelFolder + folderName))
        {
            Directory.CreateDirectory(gameLevelFolder + folderName);
        }
    }
    public static byte[] blocksA;

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
        /*
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
        */
        byte [,] blocksB = TerrainGeneration.TrueGenTerrain();
        List<byte> list = new List<byte>();
        foreach (var item in blocksB)
        {
            list.Add(item);
            writer.Write(item);
        }
        blocksA = list.ToArray();
       
        writer.Close();
        stream.Close();
        
        Debug.Log("Generado Terreno a fichero");
    }

    public void readFile(string actualLevel, out byte[] letters)
    {
        if(blocksA != null)
        {
            letters = blocksA;
        }else
        {
            System.IO.StreamReader SR = new StreamReader(gameLevelFolder + actualLevel + "/" + chuckFileSTD);
            //FileStream stream = new FileStream(gameLevelFolder + actualLevel + "/" + chuckFileSTD, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(SR.BaseStream);

        letters = new byte[reader.BaseStream.Length];
            for (uint i = 0; i < reader.BaseStream.Length; i++)
            {
                letters[i] = reader.ReadByte();
            }

            reader.Close();
            SR.Close();
        }
        //byte[] letters;
        
       // return letters;
    }
    //The last "/" between the file and the path shouldn't be used
    public void doBackupFile(bool delete, string actualLevel = "0")
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
}

