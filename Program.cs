using System;
using System.Collections.Generic;
using System.Linq;

public class ReleaseRetention
{
    public Dictionary<(string project, string environment), List<string>>
        DetermineReleasesToKeep(
        List<string> projects,
        List<string> environments,
        List<(string project, string release, string environment)> deployments,
        int numberOfReleasesToKeep)
    {
        // Dictionary to store releases to keep for each project/environment combination
        var releasesToKeep = new Dictionary<(string project, string environment), List<string>>();

        // Iterate through each project/environment combination
        foreach (var project in projects)
        {
            foreach (var environment in environments)
            {
                // Get deployments for the current project/environment combination
                var deploymentsForProjectAndEnvironment = deployments
                    .Where(d => d.project == project && d.environment == environment)
                    .ToList();

                // Get distinct releases from the deployments
                var distinctReleases = deploymentsForProjectAndEnvironment
                    .Select(d => d.release)
                    .Distinct()
                    .ToList();

                // Sort releases by deployment date (most recent first)
                var releasesSortedByDeploymentDate = distinctReleases
                    .OrderByDescending(r => deploymentsForProjectAndEnvironment
                        .Where(d => d.release == r)
                        .Max(d => d.deploymentDate))
                    .ToList();

                // Keep only the specified number of releases
                var releasesToKeepForProjectAndEnvironment = releasesSortedByDeploymentDate
                    .Take(numberOfReleasesToKeep)
                    .ToList();

                // Add the releases to keep to the dictionary
                releasesToKeep.Add((project, environment), releasesToKeepForProjectAndEnvironment);
            }
        }

        return releasesToKeep;
    }
}

