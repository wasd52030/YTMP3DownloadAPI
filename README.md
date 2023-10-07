# YTMP3DownloadAPI
把之前寫的[yt播放清單下載器](https://github.com/wasd52030/YTPlayListDownloader)其中下載的部分獨立成一隻API了，就醬

會起這個念頭主要是想把自己搓的yt播放清單下載器轉成手機用，省的每次都要在電腦上用完再摳過去，而[這位大老寫的yt library](https://github.com/Tyrrrz/YoutubeExplode)又是最穩的，
於是就想到可以出成一個api放到網路上的免費服務，這樣以後搓app的時候就可以直接用了差低

這次有用docker包container，主要是大老的yt library有用到ffmpeg，怕網路上的免費服務無法提供，只是這樣就要找能用docker deploy的服務了
