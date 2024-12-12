package com.prologin.login.Utils;

import com.prologin.login.Models.BookedSeat;

import java.util.List;

public class GetBookedSeatsMessage {
    public static String getSeatsDetails(List<BookedSeat> bookedSeats) {
        StringBuilder seatDetails = new StringBuilder();
        for (BookedSeat seat : bookedSeats) {
            seatDetails.append(String.format("Row %s, Seat %d: %s%n", seat.getSeatRow(), seat.getSeatCol(), seat.getType()));
        }
        return seatDetails.toString();
    }

}
