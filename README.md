games-cli
=========

![GitHub repo size](https://img.shields.io/github/repo-size/diademoff/games-cli)
![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/diademoff/games-cli)
![GitHub](https://img.shields.io/github/license/diademoff/games-cli)

Играй в классические игры в терминале.

<img src="https://i.imgur.com/Yovm1S1.png" alt="drawing" width="500"/>

<img src="https://i.imgur.com/Iw6sc5V.png" alt="drawing" height="330"/>

<img src="https://i.imgur.com/FJ8BFtk.png" alt="drawing" width="500"/>


Платформы:
* Windows
* Gnu Linux

Snake:
* Управление <kbd>W</kbd>, <kbd>A</kbd>, <kbd>S</kbd>, <kbd>D</kbd> или <kbd>↑</kbd>, <kbd>←</kbd>, <kbd>↓</kbd>, <kbd>→</kbd> или <kbd>K</kbd>, <kbd>H</kbd>, <kbd>J</kbd>, <kbd>L</kbd>
* <kbd>Escape</kbd> пауза
* <kbd>Пробел</kbd> ускорение

Tetris:
* <kbd>W</kbd>, <kbd>A</kbd>, <kbd>S</kbd>, <kbd>D</kbd> или <kbd>↑</kbd>, <kbd>←</kbd>, <kbd>↓</kbd>, <kbd>→</kbd>
* <kbd>Escape</kbd> пауза
* <kbd>Пробел</kbd>, <kbd>S</kbd> или <kbd>↓</kbd> для ускорения

Flappy bird
* <kbd>W</kbd>, <kbd>K</kbd>, <kbd>Пробел</kbd> или <kbd>↑</kbd>

[Скачать бинарные файлы](https://github.com/diademoff/games-cli/releases)

# Сборка из исходников
Установите `dotnet-sdk` для сборки.

```
git clone https://github.com/diademoff/games-cli && cd games-cli/games-cli
```

## Linux
Сборка и установка:
```
make && sudo make install
```

Запуск:
```
games-cli
```

## Windows
Сборка:
```
dotnet publish -r win-x64 -c Release -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true
```
