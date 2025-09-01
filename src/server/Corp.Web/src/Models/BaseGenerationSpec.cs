using TypeGen.Core.SpecGeneration;

namespace Corp.Models;

public class BaseGenerationSpec : GenerationSpec
{
    public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args)
    {
        AddBarrel(".", BarrelScope.Files); // adds one barrel file in the global TypeScript output directory containing only files from that directory
        AddBarrel(".", BarrelScope.Files | BarrelScope.Directories); // equivalent to AddBarrel("."); adds one barrel file in the global TypeScript output directory containing all files and directories from that directory

        // the following code, for each directory, creates a barrel file containing all files and directories from that directory
        IEnumerable<string> directories = GetAllDirectoriesRecursive(args.GeneratorOptions.BaseOutputDirectory)
            .Select(x => GetPathDiff(args.GeneratorOptions.BaseOutputDirectory, x));

        foreach (string directory in directories)
        {
            AddBarrel(directory);
        }

        AddBarrel(".");
    }

    private string GetPathDiff(string pathFrom, string pathTo)
    {
        var pathFromUri = new Uri("file:///" + pathFrom?.Replace('\\', '/'));
        var pathToUri = new Uri("file:///" + pathTo?.Replace('\\', '/'));

        return pathFromUri.MakeRelativeUri(pathToUri).ToString();
    }

    private IEnumerable<string> GetAllDirectoriesRecursive(string directory)
    {
        var result = new List<string>();
        string[] subdirectories = Directory.GetDirectories(directory);

        if (!subdirectories.Any()) return result;

        result.AddRange(subdirectories);

        foreach (string subdirectory in subdirectories)
        {
            result.AddRange(GetAllDirectoriesRecursive(subdirectory));
        }

        return result;
    }
}
