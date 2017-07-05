
namespace ScientificResearch.Utility
{
    public static class ReplaceBadCharOfFileName
    {
        /// <summary>
        /// 去掉文件名中的无效字符,如 \ / : * ? " < > | 
        /// </summary>
        /// <param name="fileName">待处理的文件名</param>
        /// <returns>处理后的文件名</returns>
        public static string ReplaceBadCharOfFileNames(string fileName)
        {
            fileName = fileName.Replace("\\", string.Empty);
            fileName = fileName.Replace("/", string.Empty);
            fileName = fileName.Replace(":", string.Empty);
            fileName = fileName.Replace("*", string.Empty);
            fileName = fileName.Replace("?", string.Empty);
            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace("<", string.Empty);
            fileName = fileName.Replace(">", string.Empty);
            fileName = fileName.Replace("|", string.Empty);
            fileName = fileName.Replace("+", string.Empty);
            fileName = fileName.Replace("#", string.Empty);
            fileName = fileName.Replace("$", string.Empty);

            //前面的替换会产生空格,最后将其一并替换掉
            fileName = fileName.Replace(" ", string.Empty);

            return fileName;
        }
    }
}
