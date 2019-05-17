Set-StrictMode -Version Latest

Describe 'Project Tests' -Tags 'CodeQuality' , 'Quality' {

    $root = Join-Path -Path $PSScriptRoot -ChildPath '..\..' -Resolve

    $script:ProjectFiles = @()

    Context "C# Project Files" {

        BeforeAll {

            $script:ProjectFiles = Get-ChildItem -Path $root -Filter "*.csproj" -Recurse
        }

        It "Should exist in repository'" {

            $script:ProjectFiles | Should -Not -BeNullOrEmpty
        }

        Context "Content Checks" {

            $script:ProjectFiles | ForEach-Object {

                $projectFile = $_ | Select-Object -ExpandProperty FullName

                $expectedPatterns = @( '<OutputPath>bin\$(Configuration)\</OutputPath>' )

                $expectedPatterns | ForEach-Object {

                    $pattern = $_

                    It "$pattern found in $projectFile" {

                        Get-Content $projectFile |
                            Select-String -Pattern $pattern |
                                Should -Not -BeNullOrEmpty
                    }
                }

                $removedPatterns = @( 'TODO' )

                $removedPatterns | ForEach-Object {

                    $pattern = $_

                    It "No $pattern found in $projectFile" {

                        Get-Content $projectFile |
                            Select-String -Pattern $pattern |
                                Should -BeNullOrEmpty
                    }
                }
            }
        }
    }
}
