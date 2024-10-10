dotnet-coverage collect "dotnet test src --verbosity normal --logger:trx --results-directory output/test-results" -f xml -o "output/coverage.xml"
reportgenerator -reports:"output/coverage.xml" -targetdir:"output/coverage-report" -reporttypes:Html -assemblyfilters:"-*Tests*;"
