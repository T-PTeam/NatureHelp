pipeline {
    agent any

    stages {
        stage('Клонування репозиторію') {
            steps {
                git branch: 'dev', url: 'https://github.com/T-PTeam/NatureHelp'
            }
        }

        stage('Збірка .NET Core') {
            steps {
                bat 'dotnet build "D:/Commercial Projects/NatureHelp/NatureHelp.sln" --configuration Release'
            }
        }

        stage('Збірка Angular') {
            steps {
                dir('src/View/nature-help') {
                    bat 'npm install'
                    bat 'npm run build --configuration=uk'
                }
            }
        }

        stage('Деплой на локальний сервер') {
            steps {
                script {
                    // Step 1: Ensure the target folder exists
                    bat 'if not exist "D:/Deployed/NatureHelp" mkdir "D:/Deployed/NatureHelp"'

                    // Clean up destination folder to avoid cyclic copy errors
                    bat 'if exist "D:/Deployed/NatureHelp/Server" rd /s /q "D:/Deployed/NatureHelp/Server"'
                    bat 'mkdir "D:/Deployed/NatureHelp/Server"'

                    // Log the source directory for the backend files to check if it contains any files that may conflict
                    bat 'echo Checking backend source directory... && dir "D:/Commercial Projects/NatureHelp/src/NatureHelp/bin/Release/net8.0/"'

                    // Log the destination directory for the backend files
                    bat 'echo Checking destination directory for backend... && dir "D:/Deployed/NatureHelp/Server/"'

                    // Step 3: Copy backend files to the target directory
                    bat 'xcopy /y /i "D:/Commercial Projects/NatureHelp/src/NatureHelp/bin/Release/net8.0/*" "D:/Deployed/NatureHelp/Server/"'

                    // Log the destination directory after copying backend files
                    bat 'echo Checking destination directory after backend copy... && dir "D:/Deployed/NatureHelp/Server/"'

                    // Clean up destination folder to avoid cyclic copy errors
                    bat 'if exist "D:/Deployed/NatureHelp/Server" rd /s /q "D:/Deployed/NatureHelp/UI"'
                    bat 'mkdir "D:/Deployed/NatureHelp/UI"'

                    // Log the source directory for the frontend build files to check for potential conflicts
                    bat 'echo Checking frontend source directory... && dir "D:/Commercial Projects/NatureHelp/src/View/nature-help/dist/nature-help-uk/"'

                    // Log the destination directory for the frontend build files
                    bat 'echo Checking destination directory for frontend... && dir "D:/Deployed/NatureHelp/UI/"'

                    // Step 4: Copy frontend build files to the target directory
                    bat 'xcopy /y /i "D:/Commercial Projects/NatureHelp/src/View/nature-help/dist/nature-help-uk/*" "D:/Deployed/NatureHelp/UI"'

                    // Log the destination directory after copying frontend files
                    bat 'echo Checking destination directory after frontend copy... && dir "D:/Deployed/NatureHelp/UI/"'

                    // Run startup script (if necessary)
                    bat 'D:/Deployed/NatureHelp/startup.bat'
                }
            }
        }
    }
}
