# Etapa 1: Construcción del juego en Unity
FROM gameci/unity-builder:latest AS builder

# Definir directorio de trabajo
WORKDIR /app

# Copiar archivos del proyecto
COPY . .

# Ejecutar la compilación de Unity en modo batch
RUN unity-editor -batchmode -quit -projectPath /app -buildTarget WebGL -executeMethod BuildScript.PerformBuild

# Etapa 2: Servir el juego con Nginx
FROM nginx:latest

# Copiar los archivos de la build
COPY --from=builder /app/build/WebGL /usr/share/nginx/html


EXPOSE 80


CMD ["nginx", "-g", "daemon off;"]
