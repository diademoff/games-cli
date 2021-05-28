games-cli
=========

![GitHub repo size](https://img.shields.io/github/repo-size/diademoff/games-cli)
![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/diademoff/games-cli)
![GitHub](https://img.shields.io/github/license/diademoff/games-cli)

Играй в классические игры в терминале.

<img src="https://i.imgur.com/Yovm1S1.png" alt="drawing" width="500"/>

<img src="https://i.imgur.com/Iw6sc5V.png" alt="drawing" height="300"/>

Платформы:
* Windows
* Gnu Linux

Snake:
* Управление `W`, `A`, `S`, `D`, стрелочки или vim
* `Escape` пауза
* `Space` ускорение

Tetris:
* `W`, `A`, `S`, `D` или стрелочки
* `Escape` пауза
* `Пробел` или `S` или стрелка вниз для ускорения

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
dotnet publish -r win-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true
```

# Документация
Сгенерируйте документацию с помощью [`doxygen`](https://github.com/doxygen/doxygen)

В папке репозитория выполните:
```
doxygen doxygen.conf
```

Документация сгенерирована по пути: `./docs/html/index.html`
