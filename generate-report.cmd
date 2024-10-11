dotnet-coverage collect 'dotnet test src --verbosity detailed --logger:trx --results-directory output/test-results' -f xml -o 'output/coverage.xml'
reportgenerator -reports:output/coverage.xml -targetdir:'output/coverage-report' -reporttypes:Html -assemblyFilters:-*Tests
