Este mini projeto foi desenvolvido em um PC, com Windows 10, Acer Nitro 5 AN515-51, com um pro
cessador Intel Core i7-7700HQ CPU @ 2.8GHz, na versão da Engine Unity 2018.2.2f1 (64 bit).

Para geração do pacote com os materiais usados, seguiu-se o guia fornecido pelo site da Unity,
presente em https://docs.unity3d.com/Manual/HOWTO-exportpackage.html. Desta forma, não deve ser
problema verificar o código.

O projeto pode ser rodado dentro da Unity, onde compilamos o projeto e rodamos internamente.
Além disso, pode ser necessário importar as bibliotecas de materiais usados. Há também uma ver-
são executável do jogo, presente na pasta FPSBuilded, que foi construída com o Windows como alvo
e com a arquitetura x86_64 selecionada. Para executar esta build, basta selecionar o arquivo no-
meado como FPS.

As configurações preferenciais usadas no desenvolvimento foram a resolução de 1920x1080, com o
Aspect Ratio de 16:9, na escala 1,25x. No executável, selecionando esta resolução, tudo parecia
ficar em seus devidos lugares.

Problemas conhecidos:

- Inimigos podem atirar fora de ângulo dependendo da distância em que se está deles

- Para resolver um problema de seleção no menu após o fim de uma partida, foi desabilitada a op-
ção de Lock Cursor na câmera de primeira pessoa. Porém, isto traz como consequência o problema de
o cursor aparecer concomitantemente ao gameplay, o que não foi resolvido a tempo.