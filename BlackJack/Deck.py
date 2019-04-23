'''
Deck will define a normal 52 card deck. It will deal cards, and keep track of remaining cards
'''
import random

class Deck:
    def __init__(self):
        '''
        instantiating the Deck class will reset the deck. 
        '''
        self.cards = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7, 8, 8, 8, 8, 9, 9, 9, 9, 10, 10, 10, 10, 11, 11, 11, 11, 12, 12, 12, 12, 13, 13, 13, 13]
    
    def deal_card(self):
        '''
        this is the main function of this class, to deal random cards, and keep track of remaining cards.
        '''
        rng_card_int = random.randint(1, len(self.cards))
        rng_card = self.cards[rng_card_int-1]

        #remove the card once it has been dealt
        self.cards.remove(rng_card)
        return rng_card

    #remaining_cards function is for testing/debugging purposes to ensure cards are being removed accordingly.
    def remaining_cards(self):
        return self.cards

'''
x = Deck()
print("card 1: {0}", x.deal_card()) 
print("card 2: {0}", x.deal_card())
print("Remaining cards: {0}", x.remaining_cards())
print("card 3: {0}", x.deal_card()) 
print("card 4: {0}", x.deal_card())
print("card 5: {0}", x.deal_card()) 
print("card 6: {0}", x.deal_card())
print("Remaining cards: {0}", x.remaining_cards())
print("card 7: {0}", x.deal_card()) 
print("card 8: {0}", x.deal_card())
print("card 9: {0}", x.deal_card()) 
print("card 10: {0}", x.deal_card())
print("Remaining cards: {0}", x.remaining_cards())
'''