class Home {
    Home() { }

    constructor() { }

    Productos() {
        $.get(
            "Home/Productos",
            (response) => {
                console.log(response);
                $("#ResultDeck").html(response[2]);
                $("#NavegadorDeck").html(response[1]);
                $("#InfoDeck").html(response[0]);
            }
        );
    }
}