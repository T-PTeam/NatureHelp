pipeline {
    agent any

    stages {
        stage('Клонування репозиторію') {
            steps {
                git 'https://github.com/T-PTeam/NatureHelp'
            }
        }

        stage('Збірка .NET Core') {
            steps {
                bat 'dotnet build D:\\Commercial Projects\\NatureHelp\\Backend.sln --configuration Release'
            }
        }

        stage('Запуск тестів') {
            steps {
                bat 'dotnet test D:\\Commercial Projects\\NatureHelp\\Tests.sln'
            }
        }

        stage('Збірка Angular') {
            steps {
                bat '''
                cd D:\\Commercial Projects\\NatureHelp\\View\\nature-help
                call npm install
                call npm run build --configuration=uk
                '''
            }
        }

        stage('Деплой на локальний сервер') {
            steps {
                script {
                    // Create the target folder if it doesn’t exist
                    bat 'if not exist "D:\\Deployed\\NatureHelp" mkdir "D:\\Deployed\\NatureHelp"'

                    // Copy backend files
                    bat 'xcopy /s /y "D:\\Commercial Projects\\NatureHelp\\Backend\\bin\\Release\\net8.0\\*" "D:\\Deployed\\NatureHelp\\Backend\\"'

                    // Copy frontend build
                    bat 'xcopy /s /y "D:\\Commercial Projects\\NatureHelp\\View\\nature-help\\dist\\*" "D:\\Deployed\\NatureHelp\\frontend\\"'

                    // Run startup script
                    bat 'D:\\Deployed\\NatureHelp\\startup.bat'
                }
            }
        }
    }
}
