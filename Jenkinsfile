pipeline {
    agent {
        node {
            label 'eventhub'
        }
    }
    
    environment {
        IMAGE_TAG = 'latest'
        IMAGE_NAME = 'eventhub.api'
        REGISTRY_URL = 'tranvuongduy2003'  // Replace with your registry URL
        DOCKER_NETWORK = 'eventhub'
        SONAR_TOKEN = credentials('token-eventhub-api-portal')
    }
    
    stages {   
        stage('Test') {
            steps {
                sh "cp /home/eventhub/eventhub-api/appsettings.Development.json ./source/EventHub.Presentation/appsettings.json"

                withSonarQubeEnv("SonarQube server connection") {
                    sh "docker run --rm \
                    -e SONAR_HOST_URL=${env.SONAR_HOST_URL} \
                    -e SONAR_SCANNER_OPTS='-Dsonar.projectKey=${env.SONAR_PROJECT_KEY}' \
                    -e SONAR_TOKEN=$SONAR_TOKEN \
                    -v '.:/usr/src' \
                    sonarsource/sonar-scanner-cli"
                }
            }
        }
      
        
        stage('Build') {
            steps {
                script {
                     // Clean up
                    sh 'docker image prune -a -f'

                     // Login to Docker Hub
                    withCredentials([usernamePassword(credentialsId: 'docker-hub-credentials', usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASSWORD')]) {
                        sh "echo ${DOCKER_PASSWORD} | docker login -u ${DOCKER_USER} --password-stdin"
                    }

                    // Build Docker image
                    sh "docker build -t ${REGISTRY_URL}/${IMAGE_NAME}:${IMAGE_TAG} -f source/EventHub.Presentation/Dockerfile ."
                    
                    // Push to registry
                    sh "docker push ${REGISTRY_URL}/${IMAGE_NAME}:${IMAGE_TAG}"
                    
                    // Clean up
                    sh 'docker image prune -a -f'
                }
            }
        }
        
        stage('Deploy') {
            steps {
                script {
                    // Pull latest image
                    sh "docker pull ${REGISTRY_URL}/${IMAGE_NAME}:${IMAGE_TAG}"
                    
                    // Stop and remove existing container
                    sh "docker stop ${IMAGE_NAME} || true"
                    sh "docker rm ${IMAGE_NAME} || true"
                    
                    // Run new container
                    sh """
                        docker run -d \
                        --name ${IMAGE_NAME} \
                        --network ${DOCKER_NETWORK} \
                        -p 8001:80 \
                        -e "ASPNETCORE_ENVIRONMENT=Development" \
                        -e "ASPNETCORE_URLS=http://+:80" \
                        ${REGISTRY_URL}/${IMAGE_NAME}:${IMAGE_TAG}
                    """
                }
            }
        }
    }  

    // post {
    //     always {
    //         slackSend (
    //             channel: '#jenkins',
    //             message: "${currentBuild.currentResult} Job ${env.JOB_NAME}\nbuild ${env.BUILD_NUMBER}\nFor more information: ${env.BUILD_URL}",
    //             color: 'good',
    //             tokenCredentialId: 'slack-secret' 
    //         ) 
    //     }
    // }
}