# EventHub

Tham khảo source thì cho mình xin 1 star!!!!

-----------------------------------------------------------

Please give me 1 star for reference source.

===========================================================


## 1. Clean Architecture
  - **Domain**: Containing _classes_, _abstract classes_, _interfaces_ to abstract objects in the **_Business Context_**.
  - **Infrastructor**: Containing _services_, interacting with _databases_, literally means **_"Infrastructure"_**.
  - **Usecase**: Also known as **Application Layer**, containing **Business Logic** (simply means, it is how to implement usecases, only defines **"how"** to implement usecases, does not go into detail about how that method works, this is done by **Infrastructor**).
  - **Presentation**: Process input and output of application.

# 2. CI/CD
  - The project has set up CI/CD to deploy to Azure App Service. If anyone wants to deploy to Azure, you can use it for reference.

# 3. How to run the app (Development Environment)
  - **Step 1:** Run docker compose (pay attention to the **path** to the docker-compose file, if you have **cd src** then you don't need to add **src/** to the command)
  ```
  docker-compose -f src/docker-compose.development.yml -p eventhub up -d --remove-orphans 
  ```
  - **Step 2:** Run app with **http** options
  **http** will run with the environment **Development** while **https** will run with the environment **Production**

-----------------------------------------------------------

## 1. Clean Architecture
  - **Domain**: Chứa các _classes_, _abstract classes_, _interfaces_ để trừa tượng hóa các đối tượng trong **_Business Context_**.
  - **Infrastructor**: Triển khai các _services_, tương tác với _database_, đúng với nghĩa đen là **_"Triển khai cơ sở hạ tầng"_**.
  - **Usecase**: Hay còn gọi là **Application Layer**, chứa các **Business Logic** (hiểu đơn giản là cách triển khai các usecase, chỉ define **"cách thức"** triển khai các usecase, không đi sâu vào việc cách thức đó hoạt động như thế nào, cái này đã có **Infrastructor** làm).
  - **Presentation**: Xử lý input, output của application.

# 2. CI/CD
  - Project đã setup CI/CD deploy lên Azure App Service. Nếu ai muốn deploy lên Azure có thể sử dụng để tham khảo.

# 3. Cách chạy app (Môi trường Development)
  - **Bước 1:** Chạy docker compose (để ý **đường dẫn** đến file docker-compose, nếu đã **cd src** thì không cần thêm **src/** vào câu lệnh)
  ```
  docker-compose -f src/docker-compose.development.yml -p eventhub up -d --remove-orphans 
  ```
  - **Bước 2:** Chạy app với **http**
  **http** sẽ chạy môi trường **Development** còn **https** sẽ chạy môi trường **Production**
