pipeline {
    agent any
    
    environment {
        IMAGE_TAG = 'latest'
        IMAGE_NAME = 'eventhub-api'
        REGISTRY_URL = 'tranvuongduy8423'  // Replace with your registry URL
        DOCKER_NETWORK = 'eventhub'
    }
    
    stages {       
        stage('Test') {
            steps {
                sh 'echo "Testing stage"'
            }
        }
        
        stage('Build') {
            steps {
                sh 'dotnet build ./source/EventHub.Presentation/EventHub.Presentation.csproj'
            }
        }
        
        stage('Package') {
            steps {
                script {
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
                        ${REGISTRY_URL}/${IMAGE_NAME}:${IMAGE_TAG}
                    """
                }
            }
        }
    }  
}