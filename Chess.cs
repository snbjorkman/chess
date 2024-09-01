using System;
using System.IO;
using System.Collections.Generic;
using Pieces;
using System.Diagnostics.CodeAnalysis;

class Chess{
    private Piece[,] board;

    private Piece[,] savedBoard;
    private string playerTurn = "white";
    private Dictionary<string, Piece> whitePieces;
    private Dictionary<string, Piece> blackPieces;


    public Chess(){
        //initialize board complete with all pieces
        //add each piece to its respective dictionary
        //"true board" will have white pieces on the bottom and it just prints out upside down on black turn

        board = new Piece[8, 8];

        board[0, 0] = new Rook(0, 0, "black");
        board[0, 1] = new Knight(1, 0, "black");
        board[0, 2] = new Bishop(2, 0, "black");
        board[0, 3] = new Queen(3, 0, "black");
        board[0, 7] = new Rook(7, 0, "black");
        board[0, 6] = new Knight(6, 0, "black");
        board[0, 5] = new Bishop(5, 0, "black");
        board[0, 4] = new King(4, 0, "black");

        board[7, 0] = new Rook(0, 7, "white");
        board[7, 1] = new Knight(1, 7, "white");
        board[7, 2] = new Bishop(2, 7, "white");
        board[7, 3] = new Queen(4, 7, "white");
        board[7, 7] = new Rook(7, 7, "white");
        board[7, 6] = new Knight(6, 7, "white");
        board[7, 5] = new Bishop(5, 7, "white");
        board[7, 4] = new King(3, 7, "white");

        
        for(int i=0; i<8; i++){
            board[1, i] = new Pawn(i, 1, "black");
        }

        for(int i=0; i<8; i++){
            board[6, i] = new Pawn(i, 6, "white");
        }

        for(int i = 0; i<8; i++){
            blackPieces.add(board[0, i]);
            blackPieces.add(board[1, i]);
            whitePieces.add(board[6, i]);
            whitePieces.add(board[7, i]);
        }
    }



    private void printBoard(){//prints an ascii representation of board[,] complete with axes. switches orientation based on playerTurn
        //add a tostring method to every chess piece
        
        if(playerTurn=="white"){
            Console.WriteLine("   a b c d e f g h\n");
            for(int i = 0; i<8; i++){
                Console.Write(8-i + "  ")
                for(int j = 0; j<8; j++){
                    if(board[i, j]==null){
                        Console.Write(". ");
                    }
                    else{
                        Console.Write(board[i, j]);
                    }
                }
                Console.Write(8-i + "  ");
            }
            Console.WriteLine("   a b c d e f g h\n");
        }

        if(playerTurn=="black"){
            Console.WriteLine("   h g f e d c b a\n");
            for(int i = 7; i>=0; i--){
                Console.Write(8-i + "  ")
                for(int j = 7; j>=0; j--){
                    if(board[i, j]==null){
                        Console.Write(". ");
                    }
                    else{
                        Console.Write(board[i, j]);
                    }
                }
                Console.Write(8-i + "  ");
            }
            Console.WriteLine("   h g f e d c b a\n");
        }
    }


    //coords are like a standard chess board (a ... h, 8 ... 1) but will be converted to a convenient (0 ... 7, 0 ... 7)
    private int parseX(string coord){//takes command given by player and outputs an x value (0 ... 7)
        return ((int)coord[0]-141);
    }


    private int parseY(string coord){//takes command given by player and outputs a y value (0 ... 7)
        return ((int)coord[0]-49);
    }


    private void togglePlayerTurn(){//switches player turn from white to black and vice versa
        if(playerTurn=="white"){
            playerTurn = "black";
        }
        if(playerTurn=="black"){
            playerTurn = "white";
        }
    }


    public void move(int xFrom, int yFrom, int xTo, int yTo){//updates board and coordinates for pieces involved
        if(board[yTo, xTo]!=null){
            board[yTo, xTo].capture();
        }

        board[yTo, xTo] = board[yFrom, xFrom];
        board[yFrom, xFrom].move(xTo, yTo);
        board[yFrom, xFrom] = null;
    }


    private string evalCheck(int xFrom, int yFrom, int xTo, int yTo){//returns which player is in check by the resulting move; if neither then return empty string

        bool blackChecked = false;
        bool whiteChecked = false;
        int bkx = blackPieces["king"].getX();
        int bky = blackPieces["king"].getY();
        int wkx = whitePieces["king"].getX();
        int wky = whitePieces["king"].getY();


        foreach(KeyValuePair<string, Piece> wPiece in whitePieces){
            if(wPiece.validMove(bkx, bky)){
                blackChecked = true;
            }
        }

        foreach(KeyValuePair<string, Piece> bPiece in blackPieces){
            if(bPiece.validMove(wkx, wky)){
                whiteChecked = true;
            }
        }

        if(playerTurn=="white"){
            if(whiteChecked){
                return "white";
            }
            if(blackChecked){
                evalCheckMate(xFrom, yFrom, xTo, yTo);
                return "black";
            }
        }
        if(playerTurn=="black"){
            if(blackChecked){
                return "black";
            }
            if(whiteChecked){
                evalCheckMate(xFrom, yFrom, xTo, yTo);
                return "white";
            }
        }

        return "";

    }


    private string evalUnCheck(int xFrom, int yFrom, int xTo, int yTo){//returns which player is in check by the resulting move; if neither then return empty string


        bool blackChecked = false;
        bool whiteChecked = false;
        int bkx = blackPieces["king"].getX();
        int bky = blackPieces["king"].getY();
        int wkx = whitePieces["king"].getX();
        int wky = whitePieces["king"].getY();


        foreach(KeyValuePair<string, Piece> wPiece in whitePieces){
            if(wPiece.validMove(bkx, bky)){
                blackChecked = true;
            }
        }

        foreach(KeyValuePair<string, Piece> bPiece in blackPieces){
            if(bPiece.validMove(wkx, wky)){
                whiteChecked = true;
            }
        }

        //may have to rearrange to the most relevant
        if(playerTurn=="white"){
            if(whiteChecked){
                return "white";
            }
            if(blackChecked){
                return "black";
            }
        }
        if(playerTurn=="black"){
            if(blackChecked){
                return "black";
            }
            if(whiteChecked){
                return "white";
            }
        }

        return "";

    }


    private void saveBoard(){//sets savedboard to board, then makes board a deepcopy of the old board.

        saveBoard = board;

        board = new Piece[8, 8];

        for(int i = 0; i<8; i++){
            for(int j = 0; j<8; j++){
                if(saveBoard[i, j]!=null){
                    board[i, j]==saveBoard[i, j].deepCopy();
                }
            }
        }

    }


    private void restoreBoard(){
        board = savedBoard;
    }


    private void evalCheckMate(int xFrom, int yFrom, int xTo, int yTo){//if move results in checkmate, make the move, print the final board, announce checkmate, quit game
        
        saveBoard();
        move(xFrom, yFrom, xTo, yTo);
        if(playerTurn=="black"){
            foreach(KeyValuePair<string, Piece> wPiece in whitePieces){
                foreach(KeyValuePair<(int x, int y)> move in wPiece.validMoves()){
                    if(evalUnCheck(wPiece.getX(), wPiece.getY(), x, y)!="white"){
                        restoreBoard();
                        return false;
                    }
                }
            }
        }


        if(playerTurn=="white"){
             foreach(KeyValuePair<string, Piece> bPiece in blackPieces){
                foreach(KeyValuePair<(int x, int y)> move in bPiece.validMoves()){
                    if(evalUnCheck(bPiece.getX(), bPiece.getY(), x, y)!="black"){
                        return false;
                    }
                }
            }
        }

        
        Console.WriteLine($"Checkmate! {playerTurn} wins!");
        Environment.Exit(0);
        
        return true;
        
    }


    private bool validMove(int xFrom, int yFrom, int xTo, int yTo){//verify move is within bounds and is actually the right players piece, delegate to piece validMove() method
        
        if(xFrom<0 || yFrom<0 || xTo<0 || yTo<0 || xFrom>7 || yFrom>7 || xTo>7 || yTo>7){
            return false;
        }

        if(board[yFrom, xFrom]==null || board[yTo, xTo]==null){
            return false;
        }

        if(board[yFrom, xFrom].color!=playerTurn || board[yTo, xTo]==playerTurn){
            return false;
        }

 
        if(evalCheck(xFrom, yFrom, xTo, yTo)==playerTurn){
            return false;
        }

        return board[yFrom, xFrom].validMove(xTo, yTo, board);
    }


    public void gameLoop(){//receives and handles input in a loop until checkmate
        
        while(true){
            printBoard();
            string input = Console.ReadLine();
            string[] cmd = input.Split(" ");
            int xFrom = parseX(cmd[0]);
            int yFrom = parseY(cmd[0]);
            int xTo = parseX(cmd[1]);
            int yTo = parseY(cmd[1]);

            if(validMove(xFrom, yFrom, xTo, yTo)){
                move(xFrom, yFrom, xTo, yTo);
                togglePlayerTurn();
            }
            else{
                Console.WriteLine("Invalid move, try again:");
            }
        }
    }  


    static void Main(String[] args){
        var CH = new Chess();
        CH.gameLoop();
    }
}