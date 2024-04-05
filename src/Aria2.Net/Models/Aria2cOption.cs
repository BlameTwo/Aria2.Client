using System.Text.Json.Serialization;

namespace Aria2.Net.Models;

public class Aria2cOption
{
    [JsonPropertyName("allow-overwrite")]
    public string AllowOverwrite { get; set; }

    [JsonPropertyName("allow-piece-length-change")]
    public string AllowPieceLengthChange { get; set; }

    [JsonPropertyName("always-resume")]
    public string AlwaysResume { get; set; }

    [JsonPropertyName("async-dns")]
    public string AsyncDns { get; set; }

    [JsonPropertyName("auto-file-renaming")]
    public string AutoFileRenaming { get; set; }

    [JsonPropertyName("auto-save-interval")]
    public string AutoSaveInterval { get; set; }

    [JsonPropertyName("bt-detach-seed-only")]
    public string BtDetachSeedOnly { get; set; }

    [JsonPropertyName("bt-enable-hook-after-hash-check")]
    public string BtEnableHookAfterHashCheck { get; set; }

    [JsonPropertyName("bt-enable-lpd")]
    public string BtEnableLpd { get; set; }

    [JsonPropertyName("bt-force-encryption")]
    public string BtForceEncryption { get; set; }

    [JsonPropertyName("bt-hash-check-seed")]
    public string BtHashCheckSeed { get; set; }

    [JsonPropertyName("bt-load-saved-metadata")]
    public string BtLoadSavedMetadata { get; set; }

    [JsonPropertyName("bt-max-open-files")]
    public string BtMaxOpenFiles { get; set; }

    [JsonPropertyName("bt-max-peers")]
    public string BtMaxPeers { get; set; }

    [JsonPropertyName("bt-metadata-only")]
    public string BtMetadataOnly { get; set; }

    [JsonPropertyName("bt-min-crypto-level")]
    public string BtMinCryptoLevel { get; set; }

    [JsonPropertyName("bt-remove-unselected-file")]
    public string BtRemoveUnselectedFile { get; set; }

    [JsonPropertyName("bt-request-peer-speed-limit")]
    public string BtRequestPeerSpeedLimit { get; set; }

    [JsonPropertyName("bt-require-crypto")]
    public string BtRequireCrypto { get; set; }

    [JsonPropertyName("bt-save-metadata")]
    public string BtSaveMetadata { get; set; }

    [JsonPropertyName("bt-seed-unverified")]
    public string BtSeedUnverified { get; set; }

    [JsonPropertyName("bt-stop-timeout")]
    public string BtStopTimeout { get; set; }

    [JsonPropertyName("bt-tracker-connect-timeout")]
    public string BtTrackerConnectTimeout { get; set; }

    [JsonPropertyName("bt-tracker-interval")]
    public string BtTrackerInterval { get; set; }

    [JsonPropertyName("bt-tracker-timeout")]
    public string BtTrackerTimeout { get; set; }

    [JsonPropertyName("check-certificate")]
    public string CheckCertificate { get; set; }

    [JsonPropertyName("check-integrity")]
    public string CheckIntegrity { get; set; }

    [JsonPropertyName("conditional-get")]
    public string ConditionalGet { get; set; }

    [JsonPropertyName("conf-path")]
    public string ConfPath { get; set; }

    [JsonPropertyName("connect-timeout")]
    public string ConnectTimeout { get; set; }

    [JsonPropertyName("console-log-level")]
    public string ConsoleLogLevel { get; set; }

    [JsonPropertyName("content-disposition-default-utf8")]
    public string ContentDispositionDefaultUtf8 { get; set; }

    [JsonPropertyName("continue")]
    public string Continue { get; set; }

    [JsonPropertyName("daemon")]
    public string Daemon { get; set; }

    [JsonPropertyName("deferred-input")]
    public string DeferredInput { get; set; }

    [JsonPropertyName("dht-file-path")]
    public string DhtFilePath { get; set; }

    [JsonPropertyName("dht-file-path6")]
    public string DhtFilePath6 { get; set; }

    [JsonPropertyName("dht-listen-port")]
    public string DhtListenPort { get; set; }

    [JsonPropertyName("dht-message-timeout")]
    public string DhtMessageTimeout { get; set; }

    [JsonPropertyName("dir")]
    public string Dir { get; set; }

    [JsonPropertyName("disable-ipv6")]
    public string DisableIpv6 { get; set; }

    [JsonPropertyName("disk-cache")]
    public string DiskCache { get; set; }

    [JsonPropertyName("download-result")]
    public string DownloadResult { get; set; }

    [JsonPropertyName("dry-run")]
    public string DryRun { get; set; }

    [JsonPropertyName("dscp")]
    public string Dscp { get; set; }

    [JsonPropertyName("enable-color")]
    public string EnableColor { get; set; }

    [JsonPropertyName("enable-dht")]
    public string EnableDht { get; set; }

    [JsonPropertyName("enable-dht6")]
    public string EnableDht6 { get; set; }

    [JsonPropertyName("enable-http-keep-alive")]
    public string EnableHttpKeepAlive { get; set; }

    [JsonPropertyName("enable-http-pipelining")]
    public string EnableHttpPipelining { get; set; }

    [JsonPropertyName("enable-mmap")]
    public string EnableMmap { get; set; }

    [JsonPropertyName("enable-peer-exchange")]
    public string EnablePeerExchange { get; set; }

    [JsonPropertyName("enable-rpc")]
    public string EnableRpc { get; set; }

    [JsonPropertyName("event-poll")]
    public string EventPoll { get; set; }

    [JsonPropertyName("file-allocation")]
    public string FileAllocation { get; set; }

    [JsonPropertyName("follow-metalink")]
    public string FollowMetalink { get; set; }

    [JsonPropertyName("follow-torrent")]
    public string FollowTorrent { get; set; }

    [JsonPropertyName("force-save")]
    public string ForceSave { get; set; }

    [JsonPropertyName("ftp-pasv")]
    public string FtpPasv { get; set; }

    [JsonPropertyName("ftp-reuse-connection")]
    public string FtpReuseConnection { get; set; }

    [JsonPropertyName("ftp-type")]
    public string FtpType { get; set; }

    [JsonPropertyName("hash-check-only")]
    public string HashCheckOnly { get; set; }

    [JsonPropertyName("help")]
    public string Help { get; set; }

    [JsonPropertyName("http-accept-gzip")]
    public string HttpAcceptGzip { get; set; }

    [JsonPropertyName("http-auth-challenge")]
    public string HttpAuthChallenge { get; set; }

    [JsonPropertyName("http-no-cache")]
    public string HttpNoCache { get; set; }

    [JsonPropertyName("human-readable")]
    public string HumanReadable { get; set; }

    [JsonPropertyName("keep-unfinished-download-result")]
    public string KeepUnfinishedDownloadResult { get; set; }

    [JsonPropertyName("listen-port")]
    public string ListenPort { get; set; }

    [JsonPropertyName("log-level")]
    public string LogLevel { get; set; }

    [JsonPropertyName("lowest-speed-limit")]
    public string LowestSpeedLimit { get; set; }

    [JsonPropertyName("max-concurrent-downloads")]
    public string MaxConcurrentDownloads { get; set; }

    [JsonPropertyName("max-connection-per-server")]
    public string MaxConnectionPerServer { get; set; }

    [JsonPropertyName("max-download-limit")]
    public string MaxDownloadLimit { get; set; }

    [JsonPropertyName("max-download-result")]
    public string MaxDownloadResult { get; set; }

    [JsonPropertyName("max-file-not-found")]
    public string MaxFileNotFound { get; set; }

    [JsonPropertyName("max-mmap-limit")]
    public string MaxMmapLimit { get; set; }

    [JsonPropertyName("max-overall-download-limit")]
    public string MaxOverallDownloadLimit { get; set; }

    [JsonPropertyName("max-overall-upload-limit")]
    public string MaxOverallUploadLimit { get; set; }

    [JsonPropertyName("max-resume-failure-tries")]
    public string MaxResumeFailureTries { get; set; }

    [JsonPropertyName("max-tries")]
    public string MaxTries { get; set; }

    [JsonPropertyName("max-upload-limit")]
    public string MaxUploadLimit { get; set; }

    [JsonPropertyName("metalink-enable-unique-protocol")]
    public string MetalinkEnableUniqueProtocol { get; set; }

    [JsonPropertyName("metalink-preferred-protocol")]
    public string MetalinkPreferredProtocol { get; set; }

    [JsonPropertyName("min-split-size")]
    public string MinSplitSize { get; set; }

    [JsonPropertyName("min-tls-version")]
    public string MinTlsVersion { get; set; }

    [JsonPropertyName("netrc-path")]
    public string NetrcPath { get; set; }

    [JsonPropertyName("no-conf")]
    public string NoConf { get; set; }

    [JsonPropertyName("no-file-allocation-limit")]
    public string NoFileAllocationLimit { get; set; }

    [JsonPropertyName("no-netrc")]
    public string NoNetrc { get; set; }

    [JsonPropertyName("no-want-digest-header")]
    public string NoWantDigestHeader { get; set; }

    [JsonPropertyName("optimize-concurrent-downloads")]
    public string OptimizeConcurrentDownloads { get; set; }

    [JsonPropertyName("parameterized-uri")]
    public string ParameterizedUri { get; set; }

    [JsonPropertyName("pause-metadata")]
    public string PauseMetadata { get; set; }

    [JsonPropertyName("peer-agent")]
    public string PeerAgent { get; set; }

    [JsonPropertyName("peer-id-prefix")]
    public string PeerIdPrefix { get; set; }

    [JsonPropertyName("piece-length")]
    public string PieceLength { get; set; }

    [JsonPropertyName("proxy-method")]
    public string ProxyMethod { get; set; }

    [JsonPropertyName("quiet")]
    public string Quiet { get; set; }

    [JsonPropertyName("realtime-chunk-checksum")]
    public string RealtimeChunkChecksum { get; set; }

    [JsonPropertyName("remote-time")]
    public string RemoteTime { get; set; }

    [JsonPropertyName("remove-control-file")]
    public string RemoveControlFile { get; set; }

    [JsonPropertyName("retry-wait")]
    public string RetryWait { get; set; }

    [JsonPropertyName("reuse-uri")]
    public string ReuseUri { get; set; }

    [JsonPropertyName("rpc-allow-origin-all")]
    public string RpcAllowOriginAll { get; set; }

    [JsonPropertyName("rpc-listen-all")]
    public string RpcListenAll { get; set; }

    [JsonPropertyName("rpc-listen-port")]
    public string RpcListenPort { get; set; }

    [JsonPropertyName("rpc-max-request-size")]
    public string RpcMaxRequestSize { get; set; }

    [JsonPropertyName("rpc-save-upload-metadata")]
    public string RpcSaveUploadMetadata { get; set; }

    [JsonPropertyName("rpc-secure")]
    public string RpcSecure { get; set; }

    [JsonPropertyName("save-not-found")]
    public string SaveNotFound { get; set; }

    [JsonPropertyName("save-session-interval")]
    public string SaveSessionInterval { get; set; }

    [JsonPropertyName("seed-ratio")]
    public string SeedRatio { get; set; }

    [JsonPropertyName("server-stat-timeout")]
    public string ServerStatTimeout { get; set; }

    [JsonPropertyName("show-console-readout")]
    public string ShowConsoleReadout { get; set; }

    [JsonPropertyName("show-files")]
    public string ShowFiles { get; set; }

    [JsonPropertyName("socket-recv-buffer-size")]
    public string SocketRecvBufferSize { get; set; }

    [JsonPropertyName("split")]
    public string Split { get; set; }

    [JsonPropertyName("stderr")]
    public string Stderr { get; set; }

    [JsonPropertyName("stop")]
    public string Stop { get; set; }

    [JsonPropertyName("stream-piece-selector")]
    public string StreamPieceSelector { get; set; }

    [JsonPropertyName("summary-interval")]
    public string SummaryInterval { get; set; }

    [JsonPropertyName("timeout")]
    public string Timeout { get; set; }

    [JsonPropertyName("truncate-console-readout")]
    public string TruncateConsoleReadout { get; set; }

    [JsonPropertyName("uri-selector")]
    public string UriSelector { get; set; }

    [JsonPropertyName("use-head")]
    public string UseHead { get; set; }

    [JsonPropertyName("user-agent")]
    public string UserAgent { get; set; }
}
