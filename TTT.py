
def h_blanks():
    '''print blank horizontal lines. No data is printed.'''
    print('   |   |   ')

def v_blanks():
    '''print verticle lines. No data is printed'''
    print('-----------')

def printRow(row):
    '''
    print player marker rows
    '''
    p1 = ' '+row[0].replace('#', ' ')+' '
    p2 = ' '+row[1].replace('#', ' ')+' '
    p3 = ' '+row[2].replace('#', ' ')+' '
    print(f'{p1}|{p2}|{p3}')

def printBoard(board):
    '''
    Print the entire board, along with player markers.
    '''   
    botRow = board[:3]
    midRow = board[3:6]
    topRow = board[6:]

    print('\n' * 10)
    h_blanks()
    printRow(topRow)
    h_blanks()
    v_blanks()
    h_blanks()
    printRow(midRow)
    h_blanks()
    v_blanks()
    h_blanks()
    printRow(botRow)
    h_blanks()

def chkWin(board, marker):
    '''
    Function to check whether a player has won the game. This should be called on each player move. Takes in a list parameter,
    along with the marker.
    '''
    winPattern = marker * 3
    #horizontal pattern check
    if (''.join(board[:3]) == winPattern or ''.join(board[3:6]) == winPattern or ''.join(board[6:]) == winPattern):
        return True        

    #verticle pattern check
    if(''.join([board[6], board[3], board[0]]) == winPattern or  ''.join([board[7], board[4], board[1]]) == winPattern or ''.join([board[8], board[6], board[2]]) == winPattern):
        return True

    #finally, diagonal pattern check
    if (''.join([board[6], board[4], board[2]]) == winPattern or ''.join([board[0], board[4], board[8]]) == winPattern):
        return True

def chkDraw(board):
    '''
    Function to check to the game is a draw. This should be called on each player move after check win.
    '''
    if (not '#' in board):
        return True
    else:
        return False 

def resetBoard():
    '''
    Reset game board.
    '''
    board = ['#', '#','#', '#', '#', '#', '#', '#', '#']
    return board   

def setReplay(rep):
    '''translate Y/N and return boolean'''
    if(rep.upper() == 'Y'):
        return True
    else:
        return False

def startGame():        
    '''
    1/22/2019 kh - This is a commandline tic-tac-toe game. 
    
    1) Take player 1 marker (X or O) input
    2) start game, print blank board
    3) player 1 and 2 take turns, alternating
    4) each turn check for win or draw
    5) if win, deal show winner text and request to play again.
    6) if draw, show draw text and request to play again.
    7) if play again, do not request player markers again, just start a new game by resetting variables and printing a blank board.

    '''
    #let player 1 choose marker
    p1 = input('Player 1, please choose your marker (X or O)')
    p1 = p1.upper()
    
    #assign remaining marker to player 2
    if (p1 == 'X'):
        p2 = 'O'
    else: 
        p2 = 'X'
    
    #this variable will allow players to keep playing until they decide to quit.
    replay = True
    
    #this variable will track of whose turn it is. start with player 1.
    turn = 'Player 1'
    
    #ensure the board is reset
    board = resetBoard()
    
    while replay == True:
        replayResponse = 'N'
        
        printBoard(board)
        
        markerValue = int(input(f'{turn}\'s move'))
        
        if (turn == 'Player 1'):
            board[markerValue-1] = p1
            
            #check game status. Reason I'm putting this validation here rather than beginning of while loop is so that I don't have to
            #add another if/else statement
            if(not chkWin(board,p1)):
                if(not chkDraw(board)):
                    turn = 'Player 2'
                else: 
                    printBoard(board)
                    board = resetBoard()
                    replayResponse = input('Cats game! Play again? Y/N')
                    replay = setReplay(replayResponse)
            else:
                printBoard(board)
                board = resetBoard()
                replayResponse = input(f'{turn} Wins! Play again? Y/N')
                replay = setReplay(replayResponse)
            
            
        else:
            board[markerValue-1] = p2
            
            if(not chkWin(board,p2)):
                if(not chkDraw(board)):
                    turn = 'Player 1'
                else: 
                    board = resetBoard()
                    replayResponse = input('Cats game! Play again? Y/N')
                    replay = setReplay(replayResponse)
            else:
                board = resetBoard()
                replayResponse = input(f'{turn} Wins! Play again? Y/N')
                replay = setReplay(replayResponse)
                
        
if __name__ == '__main__':
    startGame()

'''
todo
1) player can choose a spot that's already used. 
2) Consider printing square labels because it's not clear or intuitive what number is mapped to which square
3) Update variable naming, spacing, etc... to improve pylint score.
'''
