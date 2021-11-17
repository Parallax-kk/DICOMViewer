using Dicom.Imaging;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Color32 = UnityEngine.Color32;

public static class DICOMReader
{
    public static Texture3D ReadDICOM(string DICOMPath)
    {
        if (string.IsNullOrEmpty(DICOMPath))
            return null;

        if (!Directory.Exists(DICOMPath))
            return null;

        DirectoryInfo directoryInfo = new DirectoryInfo(DICOMPath);
        FileInfo[] fileInfo = directoryInfo.GetFiles("*.dcm");

        if (fileInfo.Length == 0)
            return null;

        List<Texture2D> listTex2D = new List<Texture2D>();

        foreach (FileInfo f in fileInfo)
        {
            string path = f.FullName;
            var image = new DicomImage(path);
            Texture2D tex2d = image.RenderImage().As<Texture2D>();
            listTex2D.Add(tex2d);
        }

        if (listTex2D.Count == 0)
            return null;

        Texture3D tex = Texture2D23D(listTex2D);
        return tex;
    }

    private static Texture3D Texture2D23D(List<Texture2D> listTex2D)
    {
        listTex2D.Reverse();

        int width = listTex2D[0].width;
        int height = listTex2D[0].height;
        TextureFormat format = listTex2D[0].format;
        Color32[] colors = new Color32[width * height * listTex2D.Count];

        for (int i = 0; i < listTex2D.Count; ++i)
        {
            var tex2d = listTex2D[i];
            if (tex2d.width != width || tex2d.height != height)
            {
                return null;
            }
            if (tex2d.format != format)
            {
                return null;
            }
            tex2d.GetPixels32().CopyTo(colors, width * height * i);
        }

        var tex3d = new Texture3D(width, height, listTex2D.Count, format, false);
        tex3d.SetPixels32(colors);
        tex3d.Apply();

        return tex3d;
    }
}