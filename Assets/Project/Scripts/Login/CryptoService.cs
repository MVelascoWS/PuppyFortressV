using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;

public static class CryptoService
{
    public static TripleDESCryptoServiceProvider cryptor;
    public static string initKey = "k3ytripl3d354t35t1ng53rv";
    public static string vectorIV = "qwertyui";

    public static string Encode3DES(string text)
    {
        int extra = 8 - (text.Length % 8);
        if (extra > 0)
        {
            for (int i = 0; i < extra; i++)
                text += '\0';
        }
        
        cryptor = new TripleDESCryptoServiceProvider();
        cryptor.KeySize = 192;
        cryptor.Key = Encoding.UTF8.GetBytes(initKey);
        cryptor.IV = Encoding.UTF8.GetBytes(vectorIV);

        ICryptoTransform cryptoper = cryptor.CreateEncryptor(Encoding.UTF8.GetBytes(initKey), Encoding.UTF8.GetBytes(vectorIV));
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoper, CryptoStreamMode.Write);
        cryptoStream.Write(Encoding.UTF8.GetBytes(text), 0, text.Length);
       
        byte[] encriptado = memoryStream.ToArray();
        //
        StringBuilder sb = new StringBuilder(encriptado.Length * 2);
        foreach (byte b in encriptado)
        {
            sb.Append(b.ToString("x").PadLeft(2, '0'));
        }

        return sb.ToString();
        //return System.Text.Encoding.ASCII.GetString(encriptado);
    }

    public static string Decode3DES(string text)
    {
        byte[] key = Encoding.ASCII.GetBytes(initKey);
        byte[] iv = Encoding.ASCII.GetBytes(vectorIV);
        byte[] data = StringToByteArray(text);
        byte[] enc = new byte[0];

        TripleDES tdes = TripleDES.Create();
        tdes.IV = iv;
        tdes.Key = key;
        tdes.Mode = CipherMode.CBC;
        tdes.Padding = PaddingMode.Zeros;
        ICryptoTransform ict = tdes.CreateDecryptor();
        enc = ict.TransformFinalBlock(data, 0, data.Length);

        return Encoding.ASCII.GetString(enc);
    }

	public static string EncodeBase64(string text)
	{
		byte[] bytesToEncode = Encoding.UTF8.GetBytes (text);
		string encodedText = Convert.ToBase64String (bytesToEncode);
		return encodedText;
	}

	public static string DecodeBase64(string text)
	{
		byte[] decodedBytes = Convert.FromBase64String (text);
		string decodedText = Encoding.UTF8.GetString (decodedBytes);
		return decodedText;
	}

    public static byte[] StringToByteArray(String hex)
    {
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }

        return bytes;
    }
}