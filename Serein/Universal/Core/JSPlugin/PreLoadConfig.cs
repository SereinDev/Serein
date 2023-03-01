namespace Serein.Core.JSPlugin
{
    internal class PreLoadConfig
    {
        public string[] Assemblies = { };

        /// <remarks>
        /// https://github.com/sebastienros/jint/blob/main/Jint/Options.cs
        /// </remarks>
        public bool
            AllowGetType,
            AllowOperatorOverloading = true,
            AllowSystemReflection,
            AllowWrite = true,
            Strict,
            StringCompilationAllowed = true;
    }
}