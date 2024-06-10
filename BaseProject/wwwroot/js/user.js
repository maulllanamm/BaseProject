// create connection
var connectionUserHub = new signalR
    .HubConnectionBuilder()
    .withUrl("/hubs/userCount")
    .build();

// Handler untuk menerima user dari hub
connectionUserHub.on("UpdateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerText = value.toString();
});

connectionUserHub.on("UpdateTotalActiveUsers", (value) => {
    var newCountSpan = document.getElementById("totalActiveUsersCounter");
    newCountSpan.innerText = value.toString();
});

// Metode untuk mengirim value userCount ke hub
function newWindowLoadedOnClient() {
    connectionUserHub.send("NewWindowLoaded");
}

// Mulai koneksi dan tangani kesalahan
function startConnection() {
    connectionUserHub.start()
        .then(() => {
            console.log("Connect to User hub Successful");
            newWindowLoadedOnClient();
        })
        .catch((err) => {
            console.error("Failed to connect to User hub", err);
        });
}

// Mulai koneksi setelah menetapkan handler untuk menerima pesan
connectionUserHub.onclose(() => {
    // Coba mulai kembali koneksi jika terputus
    startConnection();
});

// Panggil fungsi untuk memulai koneksi
startConnection();