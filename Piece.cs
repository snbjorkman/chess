using System.Collections.Generic;

namespace Pieces;

abstract class Piece{
    protected int x;
    protected int y;
    protected string color;

    public Piece(int xCoord, int yCoord, string pieceColor){
        x = xCoord;
        y = yCoord;
        color = pieceColor;
    }


    public void move(int xTo, int yTo){
        x = xTo;
        y = yTo;
    }


    public void capture(){
        x = -1;
        y = -1;
    }


    public int getX(){
        return x;
    }

    public int getY(){
        return y;
    }


    public Piece deepCopy(){
        Piece copy = new Piece(this.x, this.y, this.color);
        return copy;
    }


    private List<(int x, int y)> tracePath(int iHat, int jHat, Piece[,] board){
        List<(int x, int y)> moves = new List<(int x, int y)>();

        for(int i = 1; i<8; i++){
            if(x+i*iHat<0 || x+i*iHat>7 || y+i*jHat<0 || y+i*jHat>7){
                return moves;
            }
            else if(board[y+i*jHat, x+i*iHat]==null){
                moves.Add((y+i*jHat, x+i*iHat));
            }
            else if(board[y+i*jHat, x+i*iHat].color!=color){
                moves.Add((y+i*jHat, x+i*iHat));
                return moves;
            }
            else{
                return moves;
            }
        }
    }


    public abstract bool validMove(int xTo, int yTo, Piece[,] board); 

    public abstract List<(int x, int y)> validMoves(Piece[,] board);

}


class Queen : Piece{
    //returns true if there is a direct horizontal, vertical, or diagonal path to the piece in question
    public override bool validMove(int xTo, int yTo, Piece[,] board){
        
        //return false if slope is not 1, -1, 0, or infinity
        if(!((float)Math.Abs(xTo-x)/(float)Math.Abs(yTo-y)==1 || (float)Math.Abs(xTo-x)/(float)Math.Abs(yTo-y)==0 || (float)Math.Abs(yTo-y)/(float)Math.Abs(xTo-x)==0)){
            return false;
        }

        //determine if which direction the piece is in
        //use direction (we're thinking of these as vectors) to see if path is clear via for loop
        int iHat = 0;
        int jHat = 0;

        try{
            iHat = (xTo-x)/Math.Abs(xTo-x);
        }catch(Exception e){}
        try{
            jHat = (yTo-y)/Math.Abs(yTo-y);
        }catch(Exception e){}

        for(int i = 1; i<8; i++){
            try{
                if(board[y+i*jHat, x+i*iHat]==null){
                    if(y+i*jHat == yTo && x+i*iHat == xTo){
                        return true;
                    }
                }
                else if(board[y+i*jHat, x+i*iHat].color==color){
                    return false;
                }
                else if(board[y+i*jHat, x+i*iHat].color!=color){
                    if(y+i*jHat == yTo && x+i*iHat == xTo){
                        return true;
                    }
                    else{
                        return false;
                    }
                }
            }catch(Exception e){}   
        }

        return false;   
    }

    
    public override List<(int x, int y)> validMoves(Piece[,] board){
        List<(int x, int y)> moves = new List<(int x, int y)>();

        moves.AddRange(tracePath(-1, -1));
        moves.AddRange(tracePath(-1, 1));
        moves.AddRange(tracePath(1, -1));
        moves.AddRange(tracePath(1, 1));
        moves.AddRange(tracePath(-1, 0));
        moves.AddRange(tracePath(0, -1));
        moves.AddRange(tracePath(1, 0));
        moves.AddRange(tracePath(0, 1));
        
        return moves;
    }


    public override string ToString(){
        if(color=="black"){
            return "♛";
        }
        if(color=="white"){
            return "♕";
        }
    }
}


class Bishop : Piece{
    public override bool validMove(int xTo, int yTo, Piece[,] board){
        //return false if slope is not 1 or -1
        if(!((float)Math.Abs(xTo-x)/(float)Math.Abs(yTo-y)==1)){
            return false;
        }

        int iHat = 0;
        int jHat = 0;

        try{
            iHat = (xTo-x)/Math.Abs(xTo-x);
        }catch(Exception e){}
        try{
            jHat = (yTo-y)/Math.Abs(yTo-y);
        }catch(Exception e){}

        for(int i = 1; i<8; i++){
            try{
                if(board[y+i*jHat, x+i*iHat]==null){
                    if(y+i*jHat == yTo && x+i*iHat == xTo){
                        return true;
                    }
                }
                else if(board[y+i*jHat, x+i*iHat].color==color){
                    return false;
                }
                else if(board[y+i*jHat, x+i*iHat].color!=color){
                    if(y+i*jHat == yTo && x+i*iHat == xTo){
                        return true;
                    }
                    else{
                        return false;
                    }
                }
            }catch(Exception e){}
        }

        return false;   
    }


    public override List<(int x, int y)> validMoves(Piece[,] board){
        List<(int x, int y)> moves = new List<(int x, int y)>();
        
        moves.AddRange(tracePath(-1, -1));
        moves.AddRange(tracePath(-1, 1));
        moves.AddRange(tracePath(1, -1));
        moves.AddRange(tracePath(1, 1));
        
        return moves;
    }


    public override string ToString(){
        if(color=="black"){
            return "♝";
        }
        if(color=="white"){
            return "♗";
        }
    }
}


class Rook : Piece{
    public override bool validMove(int xTo, int yTo, Piece[,] board){
        
        //return false if slope is not 0 or infinity
        if(!((float)Math.Abs(xTo-x)/(float)Math.Abs(yTo-y)==0 || (float)Math.Abs(yTo-y)/(float)Math.Abs(xTo-x)==0)){
            return false;
        }

        int iHat = 0;
        int jHat = 0;

        try{
            iHat = (xTo-x)/Math.Abs(xTo-x);
        }catch(Exception e){}
        try{
            jHat = (yTo-y)/Math.Abs(yTo-y);
        }catch(Exception e){}

        for(int i = 1; i<8; i++){
            try{
                if(board[y+i*jHat, x+i*iHat]==null){
                    if(y+i*jHat == yTo && x+i*iHat == xTo){
                        return true;
                    }
                }
                else if(board[y+i*jHat, x+i*iHat].color==color){
                    return false;
                }
                else if(board[y+i*jHat, x+i*iHat].color!=color){
                    if(y+i*jHat == yTo && x+i*iHat == xTo){
                        return true;
                    }
                    else{
                        return false;
                    }
                }
            }catch(Exception e){}
        }

        return false;   
    }


    public override List<(int x, int y)> validMoves(Piece[,] board){
        List<(int x, int y)> moves = new List<(int x, int y)>();
        
        moves.AddRange(tracePath(-1, 0));
        moves.AddRange(tracePath(0, -1));
        moves.AddRange(tracePath(1, 0));
        moves.AddRange(tracePath(0, 1));
        
        return moves;
    }


    public override string ToString(){
        if(color=="black"){
            return "♜";
        }
        if(color=="white"){
            return "♖";
        }
    }
}



class Knight : Piece{
    public override bool validMove(int xTo, int yTo, Piece[,] board){
        if((xTo==x+1 && yTo==y+2) || (xTo==x-1 && yTo==y+2) || (xTo==x+2 && yTo==y+1) || (xTo==x-2 && yTo==y+1) || (xTo==x+1 && yTo==y-2) || (xTo==x-1 && yTo==y-2) || (xTo==x+2 && yTo==y-1) || (xTo==x-2 && yTo==y-1)){
            return true;
        }
        else{
            return false;
        }
    }


    private bool validKnightMove(int xDiff, int yDiff,Piece[,] board){
        try{
            if(board[y+yDiff, x+xDiff]==null){
                return true;
            }
            else if(board[y+yDiff, x+xDiff].color!=color){
                return true;
            }
        }catch(Exception e){}

        return false;
    }


    public override List<(int x, int y)> validMoves(Piece[,] board){
        List<(int x, int y)> moves = new List<(int x, int y)>();
        
        if(validKnightMove(1, 2)){moves.Add((x+1, y+2))}
        if(validKnightMove(2, 1)){moves.Add((x+2, y+1))}
        if(validKnightMove(-1, 2)){moves.Add((x-1, y+2))}
        if(validKnightMove(-2, 1)){moves.Add((x-2, y+1))}
        if(validKnightMove(1, -2)){moves.Add((x+1, y-2))}
        if(validKnightMove(2, -1)){moves.Add((x+2, y-1))}
        if(validKnightMove(-1, -2)){moves.Add((x-1, y-2))}
        if(validKnightMove(-2, -1)){moves.Add((x-2, y-1))}
        
        return moves;
    }


    public override string ToString(){
        if(color=="black"){
            return "♞";
        }
        if(color=="white"){
            return "♘";
        }
    }
}


class Pawn : Piece{
    //for now we're not implementing en passant 

    private bool moved = false;

    public override void move(int xTo, int yTo){
        x = xTo;
        y = yTo;
        moved = true;
    }

    public override bool validMove(int xTo, int yTo, Piece[,] board){
        int jHat;
        if(color=="white"){
            jHat=-1;
        }
        else{
            jHat=1;
        }

        if(moved==false){
            if(xTo==x && yTo==y+1*jHat && board[y+1*jHat, x]==null){
                return true;
            }
            if(xTo==x && yTo==y+2*jHat && board[y+2*jHat, x]==null && board[y+1*jHat, x]==null){
                return true;
            }
        }
        else if(moved==true){
            if(xTo==x && yTo==y+1*jHat && board[y+1*jHat, x]==null){
                return true;
            }
        }

        if(xTo==x+1 && yTo==y+1*jHat){
            try{
                if(board[y+1*jHat, x+1]!=null){
                    return true;
                }
            }catch(Exception e){}
        }
        else if(xTo==x-1 && yTo==y+1*jHat){
            try{
                if(board[y+1*jHat, x-1]!=null){
                    return true;
                }
            }catch(Exception e){}
        }
        else{
            return false; 
        }
    }


        public override List<(int x, int y)> validMoves(Piece[,] board){

            List<(int x, int y)> moves = new List<(int x, int y)>();

            int jHat;
            if(color=="white"){
                jHat==-1;
            }
            else{
                jHat==1;
            }

            
            try{
                if(moved==false && board[y+2*jHat, x]==null){
                    moves.Add((x, y+2*jHat));
                }
            }catch(Exception e){}

            try{
                if(board[y+jHat, x]==null){
                    moves.Add((x, y+jHat));
                }
            }catch(Exception e){}

            try{
                if(board[y+jHat, x+1].color!=color){
                    moves.Add((x+1, y+jHat));
                }
            }catch(Exception e){}

            try{
                if(board[y+jHat, x-1].color!=color){
                    moves.Add((x-1, y+jHat));
                }
            }catch(Exception e){}
            

            return moves;
        }


        public override string ToString(){
        if(color=="black"){
            return "♟";
        }
        if(color=="white"){
            return "♙";
        }
        }

}



class King : Piece{
    //for now we're not doing castling or the likes, that can be added later.

    public override bool validMove(int xTo, int yTo, Piece[,] board){
        if(Math.Abs(xTo-x)>1 || Math.Abs(yTo-y)>1){
            return false;
        }
        return true;
    }


    private List<(int x, int y)> kingMove(int xDiff, int yDiff, Piece[,] board){
        List<(int x, int y)> moves = new List<(int x, int y)>();
        try{
            if(board[y+yDiff, x+xDiff].color!=color){
                moves.Add((x+xDiff, y+yDiff));
            }
        }catch(Exception e){}

        return moves;
    }


    public override List<(int x, int y)> validMoves(Piece[,] board){
        List<(int x, int y)> moves = new List<(int x, int y)>();

        moves.AddRange(kingMove(1, 1, board));
        moves.AddRange(kingMove(1, 0, board));
        moves.AddRange(kingMove(1, -1, board));
        moves.AddRange(kingMove(0, 1, board));
        moves.AddRange(kingMove(0, -1, board));
        moves.AddRange(kingMove(-1, 1, board));
        moves.AddRange(kingMove(-1, 0, board));
        moves.AddRange(kingMove(-1, -1, board));
        
        return moves;
    }


    public override string ToString(){
        if(color=="black"){
            return "♚";
        }
        if(color=="white"){
            return "♔";
        }
        }
    
}