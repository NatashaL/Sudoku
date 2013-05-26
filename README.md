#**Sudoku**

Windows Forms Project by: 
Natasha Lazarova, Martin Ivanovski and Stevica Bozinovski

---
**Македонски** / [English](#1-description)
##1. Опис на апликацијата
Апликацијата што ја развиваме е класичната игра Sudoku, која ја проширивме застапувајќи 2 варијанти на играта: **Classic Sudoku** и **Squiggly Sudoku**.

Со цел да обезбедиме комплетно задоволство кај играчот, покрај чистиот и едноставен дизајн, имлементиравме различни алгоритми соодветни за варијантите на играта, зачувување на недовршена игра и нејзино продолжување во друго време и High Scores за секое ниво на тежина одделно.

##2. Упатство за користењe

###2.1 Нова игра

![alt text][new_game_screen]

На почетниот прозорец (слика 1) при стартување на апликацијата имаме можност да започнеме нова игра **(New game)**, да продолжиме веќе постоечка игра **(Load game)** како и да видиме листа со рекорди **(High scores)**.

Доколку сакаме да започнеме нова игра најпрво се селектира варијантата на sudoku што сакаме да решаваме. Има две понудени опции **Standard Sudoku** и **Squiggly Sudoku**. За секоја од нив се одбира едно од трите нивоа на тежина:

* Easy
* Medium
* Hard

###2.2 High Scores

Тука sе чуваат најдобрите 5 играчи, сортирани според времето на завршување на играта, за двете варијанти на судоку и сите нивоа на тежина посебно.

Податоците тука се [**_сериализирани_**](#1-description)* и се достапни и после исклучување на играта.

Исто така овозможено е бришење на сите highscores со стискање на копчето **Clear**, при што се појавува соодветно предупредување.

(ТУКА СЛИКА ОД ХАЈ СКОРС ПАНЕЛ)


Кога играчот прв пат ја вклучува апликацијата на својот компјутер, се креира _скриен_ документ во кој ќе се сериализираат неговите најдобри резултати. 
После секоја игра завршена со солиден резултат, неговото време да е подобро од најлошото во High Scores, играчот добива можност неговиот резултат да биде зачуван.
Ако тоа не го сака не мора да го направи, откажувајќи со едноставно кликање на **_No Thanks_** копчето во формата за внесување на името.

( ТУКА СЛИКА ОД ТАА ФОРМА )

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

*Регионите се означени различно за различните типови на Судоку. Додека за Squiggly судоку-то користиме бои за означување на различните региони.


##3. Претставување на проблемот

###3.1 Податочни структури

Главните податоци и [функции](#algs) за играта се чуваат во класа ```public class Sudoku``` од која пак наследуваат двете класи ```public class Standard``` и ```public class Squiggly```.

Секоја променлива и функција содржи xml summary, со детално објаснување.

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
```c#
    public class Standard : Sudoku
```
Со оваа класа го дефинираме стандардното судоку кое има 9 региони со по 9 полиња распоредени во 3x3 конфигурација.

Регионите се наједноставно одделени со две вертикални и две хоризонтални линии.

####3.1.1 Squiggly Sudoku
```c#
    public class Squiggly : Sudoku
```
Со оваа класа го дефинираме squiggly судоку-то кај кое регионите можат да бидат различни споени форми. _сепак зафаќаат по 9 полиња секој_

Регионите се одделени со обојување нивната позадина со различни бои.

###3.2 Алгоритми

##### недовршено

###3.3 GUI

##### недовршено


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
[new_game_screen]: http://igoimpeks.com/projects/sudoku/StartPanel_view.png "Слика 1"
[new_game_screen_en]: http://igoimpeks.com/projects/sudoku/StartPanel_view.png "Image 1" 