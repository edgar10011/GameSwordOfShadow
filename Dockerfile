# Etapa 1: Construcción del juego en Unity
FROM unityci/editor:ubuntu-2022.3.10f1-webgl-1 AS builder

# Definir directorio de trabajo
WORKDIR /app

# Copiar archivos del repositorio
COPY . .

# Ejecutar la compilación de Unity en modo batch
RUN unity-editor -batchmode -quit -projectPath /app -buildTarget WebGL -executeMethod BuildScript.PerformBuild

# Etapa 2: Servir el juego con Nginx
FROM nginx:latest

# Copiar los archivos generados desde la etapa anterior
COPY --from=builder /app/build/WebGL /usr/share/nginx/html

# Exponer el puerto 80
EXPOSE 80

# Iniciar Nginx
CMD ["nginx", "-g", "daemon off;"]
