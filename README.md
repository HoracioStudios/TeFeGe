# TeFeGe
---
![TeFeGeImg]

[TeFeGeImg]: Assets/UI/GraphicAssets/Tefege_Pixel_-_Grande.png "Logo TeFeGe"

## Propósito
TeFeGe ha sido diseñado y creado como caso de estudio para el servidor de [_matchmaking_] creado por [HoracioStudios].

[_matchmaking_]: https://github.com/HoracioStudios/Matchmaking-Server
[HoracioStudios]: https://github.com/HoracioStudios

## Descripción
TeFeGe se trata de un juego de género _twin-stick_ _shooter_ multijugador con perspectiva en vista cenital.

- _**Twin Stick**_: El juego se controla con un _joystick_ para moverse y otro para apuntar (en caso de usar mando), o las teclas W/A/S/D para moverse y el ratón para apuntar (en caso de emplear teclado y ratón)
- _**Shooter**_: La jugabilidad se centra en el uso de armas que disparan a la hora de atacar al oponente. Cada partida constará de 3 rondas diferentes, en las que 2 jugadores se enfrentarán en un mapa de reducido tamaño (para forzar el enfrentamiento directo entre ambos) entresí a lo largo de los 45 segundos que dura cada ronda, con el objetivo de eliminar al rival agotando sus puntos de vida, utilizando el arma y habilidad del personaje utilizado. Encaso de alcanzar el límite de tiempo, se tratará como un empate.

## Mecánicas

Las mecánicas principales del juego son:
- **Disparos y habilidades**: Cada personaje tiene un arma con sus propias balas,cadencia de disparo, munición, etcétera. Similarmente, todos cuentan con una habilidad especial propia que requiere esperar un tiempo de recarga con cada uso. Larecarga de las armas es automática, realizándose inmediatamente una vez se quedensin munición. No se puede recargar el arma manualmente.
- **Movimiento y apuntado de arma**: El movimiento del personaje y el apuntado del arma son independientes el uno del otro, una característica propia del género.
- **Rondas de tiempo limitado**: Las partidas se componen de 3 rondas de 45 segundos, y cuando se acaba el tiempo se considera un empate y empieza la siguienteronda.

## Personajes

- Manolo McFly:
![Manolo McFly]  
    - Arma: Escopeta
        - Disparo semi-automático, con corto alcance.
        - Daño: 1 punto de vida por bala.
        - Munición: 9 balas por cargador.
        - Dispara 3 proyectiles en cono, haciendo que pueda hacer mucho daño enpoco tiempo, pero con recargas frecuentes.
    - Habilidad: Cóctel molotov. 
        - Lanza un cóctel molotov que deja un charco de fuego donde impacte.
        - En caso de golpear a un jugador se rompe, pero no hace daño.
        - El charco desaparece pasados unos segundos, y hace daño a los jugadoresque entren en contacto con el mismo.
 
[Manolo McFly]: Assets/Sprites/Characters/Manolo&#32;McFly/Personaje&#32;2.png "Manolo McFly"

- Chuerk Chuerk: 
![Chuerk Chuerk]
    - Arma: Ametralladora
        - Disparo automático, con alta cadencia, poca precisión (ya que las balas sedispersan de su trayectoria con cierta aleatoriedad), pero a cambio cuentacon un alcance elevado.
        - Daño: 1 punto de vida por bala.
        - Munición: 10 balas por cargador.
    - Habilidad: Turbocuesco. 
        - Al contrario que las otras habilidades, tiene su propia carga.
        - Mantener pulsado el botón de habilidad dará un impulso de velocidadhasta que bien se suelte el botón, o bien se acabe la carga de la habilidad.
        - Activar la habilidad deja un rastro de nubes verdes a su paso, que causandaño a los enemigos, pero desaparecen pasados unos segundos.  

[Chuerk Chuerk]: Assets/Sprites/Characters/Chuerk&#32;Chuerk/Personaje&#32;1.png "Chuerk Chuerk"

- Bob Ojocojo: 
![Bob Ojocojo]
    - Arma: Pistola de goma
        - Disparo automático, con cadencia media y precisión media (dispersiónreducida), con alto alcance.
        - Daño: 1 punto de vida por bala.
        - Munición: 5 balas por cargador.
        - Las balas rebotan contra muros y obstáculos hasta 3 veces, desaparecenal siguiente, o bien tras acabarse su tiempo de vida.
    - Habilidad: Francotirador.
        - Mientras se tenga pulsado el botón de habilidad, la cámara se aleja paramostrar el mapa entero y se muestra un gráfico que indica la dirección dedisparo.
        - Al soltar el botón, se dispara una bala de alta velocidad y daño elevadoque no desaparece hasta colisionar con algún elemento del mapa.  

[Bob Ojocojo]: Assets/Sprites/Characters/Bob&#32;Ojocojo/Personaje&#32;4.png "Bob Ojocojo"

- Camomila Séstima:
![Camomila Séstima] 
    - Arma: Revólver.
        - Disparo semi-automático, con alto alcance.
        - Daño: 1 punto de vida por bala.
        - Munición: 6 balas por cargador.
    - Habilidad: _Dodge_
        - Activar la habilidad hace que Camomila se deslice en la dirección en lacual se estaba moviendo previamente durante un breve periodo de tiempo.
        - Mientras se desliza, no puede recibir daño.
        -  Al detenerse, se dispara toda la munición restante en la dirección a la cualesté apuntando, en forma de una nube de balas.
  

[Camomila Séstima]: Assets/Sprites/Characters/Camomila&#32;Sestima/Personaje&#32;3.png "Camomila Séstima"

- Bad Baby : 
![Bad Baby]
    - Arma: Micrófono
        - Disparo semi-automático, con alto alcance y una velocidad menor.
        - Daño: 1 punto de vida.
        - Munición: 5 balas por cargador.
        - Las balas flotan en círculo alrededor de Bad Baby.
        - Estas balas hacen tanto de escudo (protegiendo de balas que contactencon estas) como de munición, en ambos casos eliminando una bala delescudo.
        - Las balas situadas en el escudo no dañan a otros jugadores.
    - Habilidad: Seducción. 
        - Igual que la habilidadSniper, mantener pulsado el botón de habilidadabre un indicador de la dirección en la que se disparará, si bien no se hacezoom.
        - Soltar la habilidad disparará un corazón (a una velocidad reducida) que,al chocar con un jugador, lo fuerza a moverse hacia Bad Baby duranteunos segundos.
  
[Bad Baby]: Assets/Sprites/Characters/Bad&#32;Baby/Personaje&#32;5.png "Bad Baby"

## Niveles

El juego cuenta con un único mapa de tamaño reducido con distintas coberturas que dalugar tanto a que los jugadores puedan enfrentarse de manera directa como persiguiéndosepara sorprender al contrario.

![Mapa]

[Mapa]: Assets/Sprites/Mapa.png "Mapa"