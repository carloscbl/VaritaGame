using System.IO;
using UnityEngine;


public class TerrainFileIO
{
    private static string chuckFileSTD = "chunkData.ck";
    public static string gameLevelFolder = Path.Combine(Application.dataPath, "levels");

    public static void createFolder(string folder)
    {
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
    }

    public static void writeChunkData(string currentLevel, byte[] data)
    {
        createFolder(Path.Combine(gameLevelFolder, currentLevel));
        string path = Path.Combine(Path.Combine(gameLevelFolder, currentLevel), chuckFileSTD);
        File.WriteAllBytes(path, data);
    }

    public static void readFile(string actualLevel, int sizeChunk, out byte[] data)
    {
        string path = Path.Combine(Path.Combine(gameLevelFolder, actualLevel), chuckFileSTD);
        data = File.ReadAllBytes(path);
    }

    //The last "/" between the file and the path shouldn't be used
    public void doBackupFile(bool delete, string actualLevel = "0")
    {
        string path = Path.Combine(gameLevelFolder, actualLevel);
        string filenameWithoutExtension = Path.GetFileNameWithoutExtension(chuckFileSTD);

        string OriginalFile = Path.Combine(path, filenameWithoutExtension + ".ck");
        string FileToReplace = Path.Combine(path, filenameWithoutExtension + ".bck");
        string BackUpOfFileToReplace = Path.Combine(path, filenameWithoutExtension + ".bck2");

        if (!File.Exists(FileToReplace))
        {
            File.Copy(@"" + OriginalFile, @"" + FileToReplace);
            if (delete)
            {
                File.Delete(actualLevel + chuckFileSTD);
            }
        }
        else
        {
            if (delete)
            {
                // Replace the file.
                File.Replace(OriginalFile, FileToReplace, BackUpOfFileToReplace);
            }
            else
            {
                File.Delete(BackUpOfFileToReplace);
                File.Move(FileToReplace, BackUpOfFileToReplace);
                File.Copy(OriginalFile, FileToReplace);
            }
            //Do the new bck
        }
    }
}

