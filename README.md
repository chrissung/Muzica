Muzica
======

This project is a proof of concept to determine if Unity3d can adeptly function as a music sequencer (conclusion: it functions quite well)

There are two main screens:

- a sampler screen with 8 pads represented 8 different sounds. You can use the default drum sounds, or record your own into each pad

![Sampler screen](https://github.com/chrissung/Muzica/blob/master/screen1.jpg)

- a grid-based playback screen in which a sequencer continually loops at a desired tempo for 16 "beats", and you simply tap/click on the squares for specific samples to be triggered at given times

![Playback screen](https://github.com/chrissung/Muzica/blob/master/screen2.jpg)

The main mechanism for timing is the repeated checking of `AudioSettings.dspTime` in the `Update()` method of the MonoBehaviour singleton `PlaybackManager.cs` against the time of the next known "beat", which is a function of the current tempo.  When the current time is within a given convergence criterion (in this case, 50ms), the AudioSource for any new notes to be played are then scheduled at the time of the impending beat, and the next beat time is incremented.

[Watch a quick demo at YouTube](https://www.youtube.com/watch?v=M1Pk5mpQEWU)

Thanks to [Nathan Rosenberg](https://github.com/pianovox) for the tasty drum samples.

Licenses
--------

All source code is licensed under the [MIT-License](https://github.com/chrissung/Muzica/blob/master/LICENSE).
