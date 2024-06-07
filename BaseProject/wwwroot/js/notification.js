var connectionNotificationHub = new signalR
    .HubConnectionBuilder()
    .withUrl("/hubs/notification")
    .build();

// Handler untuk menerima notifikasi dari hub
connectionNotificationHub.on("SendMessage", (value) => {
    // Dapatkan elemen <ul>
    var messages = document.getElementById("messages");

    // Buat elemen <li> baru
    var liElement = document.createElement("li");

    // Tambahkan teks ke dalam elemen <li> baru
    var textNode = document.createTextNode(value.toString());
    liElement.appendChild(textNode);

    // Tambahkan elemen <li> baru ke dalam elemen <ul>
    messages.appendChild(liElement);
});

// Metode untuk mengirim pesan ke hub
function SendMessageOnClient() {
    // Ganti "Hello World" dengan data yang ingin Anda kirim
    connectionNotificationHub.send("SendMessage", "Hello World");
}

// Mulai koneksi dan tangani kesalahan
function startConnection() {
    connectionNotificationHub.start()
        .then(() => {
            console.log("Connect to Notification hub Successful");
        })
        .catch((err) => {
            console.error("Failed to connect to Notification hub", err);
        });
}

// Mulai koneksi setelah menetapkan handler untuk menerima pesan
connectionNotificationHub.onclose(() => {
    // Coba mulai kembali koneksi jika terputus
    startConnection();
});

// Panggil fungsi untuk memulai koneksi
startConnection();
