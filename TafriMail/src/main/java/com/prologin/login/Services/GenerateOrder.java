package com.prologin.login.Services;

import com.razorpay.Order;
import com.razorpay.RazorpayClient;
import com.razorpay.RazorpayException;
import org.json.JSONObject;

public class GenerateOrder {
    public static String placeOrderToMerchant(Integer amount , Integer bookingId) throws RazorpayException {
        RazorpayClient razorpay = new RazorpayClient("rzp_test_UsrTaVdC2mGjmn", "yhMatL98tfBjoF4Bc7qCf2ZY");

        JSONObject orderRequest = new JSONObject();
        orderRequest.put("amount", amount * 100);
        orderRequest.put("currency", "INR");
        orderRequest.put("receipt", "receipt#1");

        JSONObject notes = new JSONObject();
        notes.put("amount", amount);
//        notes.put("userId", userId);
        notes.put("bookingId", bookingId);
        orderRequest.put("notes", notes);

        try {
            // Create an order
            Order order = razorpay.orders.create(orderRequest);

            System.out.println("This is the order details");
            System.out.println(order);
            // Return the order ID
            return order.get("id").toString();
        } catch (RazorpayException e) {
            // Handle Razorpay exception
            System.out.println("Order creation failed: " + e.getMessage());
            throw e;
        }
    }
}
