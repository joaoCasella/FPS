Este mini projeto foi desenvolvido em um PC, com Windows 10, Acer Nitro 5 AN515-51, com um pro
cessador Intel Core i7-7700HQ CPU @ 2.8GHz, na vers�o da Engine Unity 2018.2.2f1 (64 bit).

Para gera��o do pacote com os materiais usados, seguiu-se o guia fornecido pelo site da Unity,
presente em https://docs.unity3d.com/Manual/HOWTO-exportpackage.html. Desta forma, n�o deve ser
problema verificar o c�digo.

O projeto pode ser rodado dentro da Unity, onde compilamos o projeto e rodamos internamente.
Al�m disso, pode ser necess�rio importar as bibliotecas de materiais usados. H� tamb�m uma ver-
s�o execut�vel do jogo, presente na pasta FPSBuilded, que foi constru�da com o Windows como alvo
e com a arquitetura x86_64 selecionada. Para executar esta build, basta selecionar o arquivo no-
meado como FPS.

As configura��es preferenciais usadas no desenvolvimento foram a resolu��o de 1920x1080, com o
Aspect Ratio de 16:9, na escala 1,25x. No execut�vel, selecionando esta resolu��o, tudo parecia
ficar em seus devidos lugares.

Problemas conhecidos:

- Inimigos podem atirar fora de �ngulo dependendo da dist�ncia em que se est� deles

- Para resolver um problema de sele��o no menu ap�s o fim de uma partida, foi desabilitada a op-
��o de Lock Cursor na c�mera de primeira pessoa. Por�m, isto traz como consequ�ncia o problema de
o cursor aparecer concomitantemente ao gameplay, o que n�o foi resolvido a tempo.