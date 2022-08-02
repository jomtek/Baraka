# Baraka
<img src="https://i.imgur.com/XXR0l4o.png" alt="Nihil icon" align=right width=200px />

## Is this project over ?
The project was permanently suspended because of an internal WPF bug concerning the FontFamily class. More specifically, this class leaks 32 bytes `UnmanagedMemoryStream` objects when it is used from a relative font path. As a result, the RAM keeps growing each time a new mushaf page is displayed, without the garbage collector making its job. A more recent technology needs to be used, and I personally refuse to push Baraka further as its main goal was to display an interactive mushaf.<br><br>
However, a workaround might still be available, for whoever wills to spend time on trying to fix the memory leak :
https://github.com/quicoli/MaterialDesignThemes.CustomResources

## A beautiful quran app
Baraka is a free software made for reading or studying the quran. It allows you to stream recitations from more than 25 reciters and read among a hundred quran translations. Fast quran browser included.<br>
We are working on a digital and interactive Madani mus'haf, as well as multiple training modes or games.<br><br>
[**Download (experimental build)**](https://github.com/Jomtek/Baraka/releases/tag/test)<br>

[![Windows](https://badgen.net/badge/icon/windows?icon=windows&label)]()
[![GitHub contributors](https://img.shields.io/github/contributors/Jomtek/Baraka.svg)](https://github.com/Jomtek/Baraka/graphs/contributors/)
[![Open Source Love](https://badges.frapsoft.com/os/v3/open-source.png?v=103)](https://github.com/ellerbrock/open-source-badges/)

## What does it look like ?
<table border="0">
 <tr>
    <td><img src="https://i.imgur.com/QF84R4l.png"></td>
    <td><img src="https://i.imgur.com/eL57wFe.png"></td>
 </tr>
</table>

## Contribute
We are actively looking for contributors. We need translators, webmasters, API developers, C# and MVVM WPF developers.<br>
<a href="mailto: baraka.support@protonmail.com">Please contact us !</a>
