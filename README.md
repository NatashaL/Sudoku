#**Sudoku**

Windows Forms Project by: 
Natasha Lazarova, Martin Ivanovski and Stevica Bozinovski
[**Binary Release**](https://github.com/NatashaL/Sudoku/blob/master/Sudoku/bin/Release/Sudoku.exe)

---
**Македонски** / [English](#1-description)
##1. Опис на апликацијата
Апликацијата што ја развиваме е класичната игра Sudoku, која ја проширивме застапувајќи 2 варијанти на играта: [**Standard Sudoku**](#311-standard-sudoku) и [**Squiggly Sudoku**](#312-squiggly-sudoku).

Со цел да обезбедиме комплетно задоволство кај играчот, покрај чистиот и едноставен дизајн, имлементиравме различни алгоритми соодветни за варијантите на играта, зачувување на недовршена игра и нејзино продолжување во друго време и High Scores за секое ниво на тежина одделно.

##2. Упатство за користењe

###2.1 Нова игра

![alt text][new_game_screen]

На почетниот прозорец (слика 1) при стартување на апликацијата имаме можност да започнеме нова игра **(New game)**, да продолжиме веќе постоечка игра [**(Load game)**](#24-load-game) како и да видиме листа со рекорди [**(High scores)**](#22-high-scores).

Доколку сакаме да започнеме нова игра најпрво се селектира варијантата на sudoku што сакаме да решаваме. Има две понудени опции [**Standard Sudoku**](#311-standard-sudoku) и [**Standard Sudoku**](#311-standard-sudoku). За секоја од нив се одбира едно од трите нивоа на тежина:

* Easy
* Medium
* Hard

###2.2 High Scores

Тука sе чуваат најдобрите 5 играчи, сортирани според времето на завршување на играта, за двете варијанти на судоку и сите нивоа на тежина посебно.

![alt text][HS_panel]

После секоја игра завршена со солиден резултат, неговото време да е подобро од најлошото во High Scores, играчот добива можност неговиот резултат да биде зачуван.
Ако тоа не го сака не мора да го направи, откажувајќи со едноставно кликање на **_No Thanks_** копчето во формата за внесување на името.

![alt text][enter_HS]

Податоците се [**_сериализирани_**](#32-)* и се достапни и после исклучување на играта.

Исто така овозможено е бришење на сите highscores со стискање на копчето **Clear**, при што се појавува соодветно предупредување.

Штом се променат High Scores, тие се ажурираат само во извршната верзија, а по затворањето на апликацијата новата верзија од резултатите се запишува врз старата.



######недовршено

###2.3 Save game
За зачувување на моменталната игра, направивме да нема експлицитно копче за кликање, туку во моментот кога ќе сака играчот да замине од апликацијата или да премине во друг поглед (панел), да му се понуди опција за зачувување на недовршената игра.

###2.4 Load Game
За да продолжеме веќе зачувана игра, во главното мени одбираме **Load game**. Зачуваната игра, _доколку има таква_ веднаш продолжува, во спротивно се прикажува соодветна порака.

###2.5 Правила
#####Правилата се едноставни:

* Не смее да има два или повеќе исти броја во една колона
* Не смее да има два или повеќе исти броја во еден ред
* Не смее да има два или повеќе исти броја во еден регион*

######*Регионите се означени различно за различните типови на [**Sudoku**](#311-standard-sudoku).


##3. Претставување на проблемот

###3.1 Податочни структури

Главните податоци и [функции](#33-) за играта се чуваат во класа ```public class Sudoku``` од која пак наследуваат двете класи ```public class Standard``` и ```public class Squiggly```.

Секоја променлива и функција содржи **xml _summary_**, со детално објаснување.

```c#
public class Sudoku
    {
        /// <summary>
        /// Describes which fields of the grid are to be shown to the player.
        /// Starting grid to be solved by player.
        /// </summary>
        public int[,] mask;
        /// <summary>
        /// Solution of the Sudoku
        /// </summary>
        public int[,] solution;
        /// <summary>
        /// The current playing grid, as filled by player.
        /// </summary>
        public int[,] userGrid;
        /// <summary>
        /// Determines the look of the grid
        /// </summary>
        public int[,] scheme;
        /// <summary>
        /// Difficulty of game
        /// </summary>
        public Difficulty diff;
        public int[,] rows;
        public int[,] cols;
        public int[,] groups;
        /// <summary>
        /// number of seconds the player has been playing
        /// </summary>
        public int ticks;
}
```

####3.1.1 Standard Sudoku
![alt text][Standard_sudoku]
```c#
    public class Standard : Sudoku
```
Со оваа класа го дефинираме стандардното судоку кое има 9 региони со по 9 полиња распоредени во 3x3 конфигурација.

#####Регионите се наједноставно одделени со две вертикални и две хоризонтални линии.

####3.1.2 Squiggly Sudoku
![alt text][Squiggly_sudoku]
```c#
    public class Squiggly : Sudoku
```
Со оваа класа го дефинираме squiggly судоку-то кај кое регионите можат да бидат различни споени форми. _сепак зафаќаат по 9 полиња секој_

#####Регионите се одделени со обојување нивната позадина со различни бои.

###3.2 Сериализација
За некои податоци да бидат достапни и после терминација на програмата, искористивме бинарна сериализација.

Кога играчот прв пат ја вклучува апликацијата на својот компјутер, се креира _скриен_ документ во `<root>/Users/[USER]/App Data/Roaming/`. Штом се променат High Scores, тие се ажурираат само во извршната верзија, а за време на затворањето на апликацијата, новата верзија од резултатите се сериализираат во фајлот `HighScores.hs`.

На ист принцип е изведено и зачувувањето на недовршена игра, но овој пат во фајлот `Sudoku.oku`.

За еден објект да можеме да го сериализираме, потребно е класата од која е инстанциран да биде сериализабилна.
```c#
    [Serializable]
    public class HighScores{}
```
```c#
    [Serializable]
    public class Sudoku{}
```
За читање на веќе внесени податоци во овие фајлови се повикува методот `Deserialize();` со FileStream од содржината на сериализираниот фајл како аргумент. Излезот од овој метод се кастира како соодветен објект и се доделува на веќе инстанциран `null` објект од иста класа. 

###3.3 Алгоритми

За да биде целосна играта на судоку имплементиравме различни алгоритми за генерирање и валидирање на успешно решение.

####3.3.1 Почетна состојба
`InitGrid();`
Со повикување на оваа фукнција прво се пополнува првата редица и првата колона со случајни броеви, запазувајќи го правилото да нема две исти соодветно, потоа се повикуваат останатите методи.

####3.3.2 Решавање
#####`SolveGrid();`
Оваа функција го решава [**Standard Sudoku**](#311-standard-sudoku) со пополнување на полињата кои за дадената почетна состојба имаат најмал број на можни вредности. Тука за проверка се користат 3 функции `IsInRow();`, `IsInCol();` и `IsIn3x3();`. Полињата што остануваат се пополнуваат користејќи ја истата техника за генерирање на пермутации рекурзивно.

#####`solve();`
Оваа функција решава [**Squiggly Sudoku**](#312-squiggly-sudoku). Тука алгоритмот е поедноставен со тоа што се користи само рекурзивната техника за генерирање на пермутации, кои не носат кон решението.
     
####3.3.3 Одстранувае на полиња
#####`Blanker();`
Оваа функција прима како аргумент решена матрица од погорните функции, и во зависност од одбраната тежина враќа нова матрица со соодветен број на одстранети полиња, за играчот да ги пополнува.

Прво се бира случајна позиција во матрицата, се бриши вредноста и се повикува соодветната функција за решавање со која се проверува дали новодобиената матрица има уникатно решение. Ако има и не е постигнат саканиот број на празни полиња, постапката продолжува се додека истиот не се постигне. Во моментот кога ќе се најде повеќе од едно решение, вредноста на последното избришано поле се враќа и се бира друго за бришење и постапката продолжува.

####3.3.3 Валидација на корисничко решение
#####`IsSolved();`
Кога сите полиња се пополнети, се повикува оваа функција која се придржува на [правилата](##25-) за да одреди дали дадената игра е точно решена.

###3.4 GUI

За претставување на матрицата за судоку користевме dataGridView контрола со измени на предодредените карактеристики.


---
---


**English** / [Македонски](#1---)
##1. Description
This application that we are developing is the classic game of Sudoku, featuring 2 variants: **Classic Sudoku** and **Squiggly Sudoku**.

For complete player satisfaction, along with the clean and simple design, we implemented various algorithms suitable for the variants of the game, saving an unfinished game and continuing it at a later time as well as High Scores for the variants and levels individually.

##2. How to play
###2.1 New Game
![alt text][new_game_screen_en]

On the main window (image 1) you have the ability to start a **New Game**, to **Load** an already saved game, or to view the **High Scores**

If you want to start a new game with different settings than the pre-defined, you can select a different **mode** and **difficulty** on the right hand side of this window.
#License
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

[GNU General Public License](https://github.com/NatashaL/Sudoku/blob/master/COPYING.md)

[new_game_screen]: http://igoimpeks.com/projects/sudoku/StartPanel_view.png "Слика 1"
[HS_panel]: http://igoimpeks.com/projects/sudoku/HighScores_panel.png "Слика 2"
[enter_HS]: http://igoimpeks.com/projects/sudoku/enter_HS.png "Слика 3"
[Standard_sudoku]: http://igoimpeks.com/projects/sudoku/standard.png "Слика 4"
[Squiggly_sudoku]: http://igoimpeks.com/projects/sudoku/squiggly.png "Слика 5"
[new_game_screen_en]: http://igoimpeks.com/projects/sudoku/StartPanel_view.png "Image 1" 



