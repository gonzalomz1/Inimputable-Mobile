# Inimputable Mobile

## Descripción General
**Inimputable Mobile** es un juego de supervivencia y acción desarrollado en Unity para plataformas móviles. El objetivo principal del jugador es sobrevivir a oleadas de enemigos ("Turros") mientras completa diversos objetivos en un entorno hostil. El juego cuenta con sistemas de dificultad progresiva y mecánicas de combate.

## Estado Actual del Proyecto
El proyecto se encuentra en una fase de desarrollo activo, con las mecánicas centrales ("Core Loop") ya implementadas. Actualmente funcional:
- **Sistema de Combate**: El jugador puede defenderse utilizando armas.
- **Enemigos (Turros)**: Implementación de IA para enemigos con variantes de ataque a distancia y cuerpo a cuerpo.
- **Dificultad Dinámica**: El juego escala la dificultad basándose en el tiempo de supervivencia (`SurvivalDifficultyManager`), ajustando la frecuencia y tipo de enemigos.
- **Gestión de Estados**: Transiciones entre menús, juego y condiciones de victoria/derrota gestionadas por una arquitectura de Managers.
- **Persistencia**: Sistema básico de reinicio de estados y control de flujo de juego.

## Tecnologías Aplicadas
El proyecto utiliza un stack tecnológico robusto orientado a la performance en móviles:
- **Motor**: Unity (C#).
- **Renderizado**: Universal Render Pipeline (URP) para gráficos optimizados y efectos visuales modernos.
- **UI**: TextMesh Pro para interfaces de usuario nítidas y escalables.
- **Arquitectura**:
  - **Managers Centralizados**: `GameManager`, `AudioManager`, `CanvasManager`, `InputManager` para desacoplar sistemas.
  - **Object Pooling**: Sistema `ObjectPooler` para la gestión eficiente de memoria al instanciar enemigos y proyectiles, crucial para móviles.
  - **Event System**: Comunicación desacoplada entre componentes.

## Funcionamiento General
El juego opera bajo una arquitectura dirigida por **Managers** que controlan el flujo global:
1.  **Game Loop**: El `GameManager` orquesta el inicio de la partida, el control de pausas y el fin del juego.
2.  **Spawning**: El `Spawner` utiliza el sistema de Object Pooling para generar enemigos en oleadas controladas.
3.  **Objetivos**: Sistema modular de objetivos (ej. `SurviveMinutesObjective`) que dicta las condiciones de victoria de cada nivel.
4.  **Interacción**: El `InputManager` procesa las entradas del jugador (táctiles/virtual joystick) y las traduce en acciones (movimiento, disparo).

## Futuras Mejoras y Roadmap
Para llevar el proyecto al siguiente nivel ("Release Candidate"), se sugieren las siguientes áreas de mejora:

### Contenido
- **Variedad de Enemigos**: Introducir nuevos tipos de enemigos con patrones de ataque únicos (ej. Tanks, Kamikazes).
- **Sistema de Progresión**: Implementar un árbol de habilidades o tienda de mejoras permanentes (Meta-game).
- **Nuevos Niveles/Habitaciones**: Expandir el `RoomManager` para generación procedimental o múltiples mapas.

### Técnico
- **Serialización**: Implementar un sistema robusto de Guardado/Carga (Save System) para persistir el progreso del jugador entre sesiones.
- **Optimización**: Profiling profundo para asegurar 60 FPS estables en dispositivos de gama media/baja. Implementación de Addressables para gestión de memoria.
- **Servicios**: Integración de Analytics y Crashlytics para monitorear el comportamiento de los usuarios y errores en producción.

---
*Documentación generada automáticamente por Antigravity AI.*
