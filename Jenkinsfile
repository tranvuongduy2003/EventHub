pipeline {
    agent {
        node {
            label 'eventhub'
        }
    }
    
    environment {
        IMAGE_TAG = 'latest'
        IMAGE_NAME = 'eventhub-api'
        REGISTRY_URL = 'tranvuongduy2003'  // Replace with your registry URL
        DOCKER_NETWORK = 'eventhub'
    }
    
    stages {   
        stage('Prepare') {
            steps {               
                script {
                    // Create .env file
                    sh """
                    cat > .env << 'EOF'
    ConnectionStrings__DefaultConnectionString=${env.ConnectionStrings__DefaultConnectionString}
    ConnectionStrings__CacheConnectionString=${env.ConnectionStrings__CacheConnectionString}
    ConnectionStrings__AzureSignalRConnectionString=${env.ConnectionStrings__AzureSignalRConnectionString}
    JwtOptions__Secret=${env.JwtOptions__Secret}
    JwtOptions__Issuer=${env.JwtOptions__Issuer}
    JwtOptions__Audience=${env.JwtOptions__Audience}
    SeqConfiguration__ServerUrl=${env.SeqConfiguration__ServerUrl}
    MinioStorage__Endpoint=${env.MinioStorage__Endpoint}
    MinioStorage__AccessKey=${env.MinioStorage__AccessKey}
    MinioStorage__SecretKey=${env.MinioStorage__SecretKey}
    Authentication__Google__ClientSecret=${env.Authentication__Google__ClientSecret}
    Authentication__Google__ClientId=${env.Authentication__Google__ClientId}
    Authentication__Facebook__ClientSecret=${env.Authentication__Facebook__ClientSecret}
    Authentication__Facebook__ClientId=${env.Authentication__Facebook__ClientId}
    EmailSettings__Email=${env.EmailSettings__Email}
    EmailSettings__Password=${env.EmailSettings__Password}
    HangfireSettings__Storage__ConnectionString=${env.HangfireSettings__Storage__ConnectionString}
    EOF
                    """
                }
            }
        }

        stage('Test') {
            steps {
                sh 'echo "Testing stage"'
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
                        --env-file .env \
                        -p 8001:80 \
                        -e "ASPNETCORE_ENVIRONMENT=Development" \
                        -e "ASPNETCORE_URLS=http://+:80" \
                        ${REGISTRY_URL}/${IMAGE_NAME}:${IMAGE_TAG}
                    """
                }
            }
        }
    }  
}