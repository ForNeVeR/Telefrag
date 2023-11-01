Telefrag [![Status Zero][status-zero]][andivionian-status-classifier]
========
Telefrag is a Git repository tagging solution.

Sometimes, you need to process the whole repository to find places where a certain characteristic was changed, and tag these points. Telefrag will help you to do it.

In particular, it may help if you want to tag versions in a software repository that has no version tags, but you know a way to determine a version of every commit.

The Algorithm
-------------
Telefrag works in two stages.

1. Perform BFS search across all the commits connected to the repository `HEAD` (or other configured starting point). For each commit, calculate the chosen characteristic. If the characteristic changes between the current commit and its parent, put the parent and the characteristic value into a multi-value map. For example, if the characteristic is _version_, this stage may result in the following map:

   ```json
   {
       "1.0": ["commit5"],
       "0.9": ["commit2", "commit3"]
   }
   ```
   
   It may be a result of the following commit graph:

   ```
   * commit6: 1.1
   * commit5: 1.0
   |\
   | * commit4: 1.0 
   * | commit3: 0.9
   |/
   * commit2: 0.9
   * commit1: 0.9
   ```
   
   As you can see, it's possible that the characteristic has changed in several points (e.g., in this case between `commit2` and `commit4`, and separately between `commit2` and `commit3`).
   
2. Resolve the resulting conflicts: if there are several change points for a single characteristic value (such as for `1.1` in the example above), choose a commit closest to `HEAD` among them (by performing a simple parallel BFS from each commit until the different searches meet).

  In this example, we should end up with `"0.9": "commit3"`. 

   If the common commit is found, and it's among the commits in question, mark it as the resolution: it was the latest commit bearing a certain version. If the commit is not found (e.g., the commits are in totally different branches), then it means the version was incremented several times in different branches, and manual resolution will be required.

As a result, Telefrag will save a data file marking all the solutions found, and will provide the means to apply these tags to the actual Git repository for further use.

Documentation
-------------

- [License (MIT)][docs.license]
- [Code of Conduct (adapted from the Contributor Covenant)][docs.code-of-conduct]

[andivionian-status-classifier]: https://github.com/ForNeVeR/andivionian-status-classifier#status-zero-
[docs.code-of-conduct]: CODE_OF_CONDUCT.md
[docs.license]: LICENSE.md
[status-zero]: https://img.shields.io/badge/status-zero-lightgrey.svg
