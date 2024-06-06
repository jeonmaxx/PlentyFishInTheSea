using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeleteSave : MonoBehaviour
{
    public string saveFilePath;

    public void DeleteFile()
    {
        if(saveFilePath != null && File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
    }
}
