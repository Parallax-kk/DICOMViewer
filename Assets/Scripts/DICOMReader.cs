using Dicom.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Color32 = UnityEngine.Color32;

public static class DICOMReader
{
    public static (Texture3D, Texture2D[]) ReadDICOM(string DICOMPath)
    {
        if (string.IsNullOrEmpty(DICOMPath))
            return (null, null);

        if (!Directory.Exists(DICOMPath))
            return (null, null);

        DirectoryInfo directoryInfo = new DirectoryInfo(DICOMPath);
        FileInfo[] fileInfo = directoryInfo.GetFiles("*.dcm");

        if (fileInfo.Length == 0)
            return (null, null);

        Texture2D[] arrayTex2D = new Texture2D[fileInfo.Length];
        for (int i = 0; i < fileInfo.Length; i++)
        {
            string path = fileInfo[i].FullName;
            var image = new DicomImage(path);
            var data = image.PixelData;
            Texture2D tex2d = image.RenderImage().As<Texture2D>();
            arrayTex2D[i] = tex2d;
        }

        if (arrayTex2D.Length == 0)
            return (null, null);

        Texture3D tex = Texture2D23D(arrayTex2D);
        return (tex, arrayTex2D);
    }

    private static Texture3D Texture2D23D(Texture2D[] arrayTex2D)
    {
        arrayTex2D.Reverse();

        int width = arrayTex2D[0].width;
        int height = arrayTex2D[0].height;
        TextureFormat format = arrayTex2D[0].format;
        Color32[] colors = new Color32[width * height * arrayTex2D.Length];

        for (int i = 0; i < arrayTex2D.Length; ++i)
        {
            var tex2d = arrayTex2D[i];
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

        var tex3d = new Texture3D(width, height, arrayTex2D.Length, format, false);
        tex3d.SetPixels32(colors);
        tex3d.Apply();

        return tex3d;
    }
}