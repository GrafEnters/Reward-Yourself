public static class TimeUtils {
    private const int DAY = 24 * 60 * 60;
    private const int HOUR = 60 * 60;
    private const int MINUTE = 60;

    public static string GetStrFromSeconds(int seconds) {
        if (seconds == 0) {
            return "0 seconds";
        }

        int days = seconds / DAY;
        seconds %= DAY;
        int hours = seconds / HOUR;
        seconds %= HOUR;
        int minutes = seconds / MINUTE;
        seconds %= MINUTE;

        if (days > 0) {
            return $"{days}d {hours}h {minutes}min";
        }

        if (hours > 0) {
            return $"{hours}h {minutes}min {seconds}s";
        }

        if (minutes > 0) {
            return $"{minutes}min {seconds}s";
        }

        return $"{seconds}s";
    }

    public static string GetShortStrFromSeconds(int seconds) {
        if (seconds == 0) {
            return "0";
        }

        int days = seconds / DAY;
        seconds %= DAY;
        int hours = seconds / HOUR;
        seconds %= HOUR;
        int minutes = seconds / MINUTE;
        seconds %= MINUTE;

        if (days > 0) {
            return $"{days}d {hours}h {minutes}min";
        }

        if (hours > 0) {
            return $"{hours}h {minutes}min";
        }

        if (minutes > 0) {
            return $"{minutes}min";
        }

        return "0";
    }
}